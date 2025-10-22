using System.Text.Json;
using Hydra.Domain.Models.Hydra;
using Hydra.Domain.Models.Lucene;
using Hydra.Infrastructure.Services.Lucene;
using Lucene.Net.Documents;

namespace Hydra.Infrastructure.Services.Hydra;

public class FontsManager
{
    private readonly LuceneDownloads _downloads = new();
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public FontsManager(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("Hydra-Library");
    }

    public async Task AddOrUpdateFont(string link)
    {
        using var response = await _client.GetAsync(link, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Request failed with status code {(int)response.StatusCode} ({response.StatusCode}).");

        var contentType = response.Content.Headers.ContentType?.MediaType;

        await using var stream = await response.Content.ReadAsStreamAsync();

        using var doc = await JsonDocument.ParseAsync(stream);
        var root = doc.RootElement;

        var name = root.GetProperty("name").GetString();
        var downloadsProp = root.GetProperty("downloads");

        Console.WriteLine($"Processing font: {name}");

        List<DownloadDocument> docs = new List<DownloadDocument>();
            
        var downloadStream = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(downloadsProp));
        await foreach (var download in JsonSerializer.DeserializeAsyncEnumerable<DownloadLink>(
                           downloadStream, _jsonOptions))
        {
            if (download == null)
                continue;
                
            docs.Add(download.ToDownloadDocument(name!));
        }

        await _downloads.AddOrUpdateBulkAsync(docs, x => (Document)x);

        Console.WriteLine("✅ Font data processed incrementally.");
    }
    
    public Task<IEnumerable<string>> GetAllSources()
        => Task.FromResult(_downloads.GetAllSources());
    
    public IEnumerable<DownloadDocument> GetLinksAsync(string name)
        => _downloads.GetLinksAsync(name);
}
