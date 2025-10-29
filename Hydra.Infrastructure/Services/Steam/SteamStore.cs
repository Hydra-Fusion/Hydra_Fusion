using Hydra.Domain.Models.Hydra.Api;
using Hydra.Domain.Models.Internal.Catalogue;
using Hydra.Domain.Models.Internal.Details;
using Hydra.Domain.Models.Lucene;
using Hydra.Infrastructure.Interfaces.Refit;
using Hydra.Infrastructure.Services.Lucene;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers.Surround.Parser;
using Microsoft.Extensions.Caching.Memory;
using SteamStorefrontAPI;

namespace Hydra.Infrastructure.Services.Steam;

public class SteamStore
{
    private readonly LuceneStore _luceneStore;
    private readonly IHydra _hydra;
    private readonly IProtonDb _protonDb;
    private IMemoryCache _cache;

    public SteamStore(IProtonDb protonDb, IHydra hydra, IMemoryCache cache)
    {
        _luceneStore = new LuceneStore("Steam", "Index/Stores/Steam");
        _protonDb = protonDb;
        _hydra = hydra;
        _cache = cache;
    }

    public async Task<Details?> GetAppDetailsAsync(uint appid)
    {
        var result = await AppDetails.GetAsync((int)appid);
        
        var proton = await _protonDb.GetCompatibilityProton((int)appid);
        
        var response = new Details(result, proton.Content);
 

        return response;
    }

    
    public async Task<Products> SearchAsync(string query, int page, int maxResults = 20)
    {
        if (page <= 0)
            throw new ArgumentException("page must be greater than zero");
        
        List<Product> games = new();

        int pagesPerApiCall = 100 / maxResults;
        
        int blockIndex = (page - 1) / pagesPerApiCall;

        int pageCurrentApiPage = (blockIndex * pagesPerApiCall) + 1;

        if (_cache.TryGetValue($"Steam-{QueryParser.Parse(query)}-{pageCurrentApiPage}", out int count))
        {
            var search = _luceneStore.Search(query, page, maxResults);
            return search.ConvertToProducts(count, maxResults);
        }

        var responseSteam = await _hydra.SearchAsync(new SearchHydraRequest
        {
            Title = query,
            Take = 100,
            Skip = blockIndex * 100
        });

        if (responseSteam.IsSuccessStatusCode)
        {
            var search = responseSteam.Content;

            List<GameDocument> products = search.Edges.Select(x => new GameDocument(x.ObjectId)
            {
                Title = x.Title,
                Categories = string.Join(',', x.Genres),
                Cover = x.LibraryImage
            }).ToList();

            await _luceneStore.AddOrUpdateBulkAsync(products
                .Select(x => (GameDocument)x)
                .ToList(), document => (Document)document);

            _cache.Set($"Steam-{QueryParser.Parse(query)}-{pageCurrentApiPage}", search!.Count, DateTimeOffset.UtcNow.AddMinutes(30));

            return new()
            {
                CurrentPage = page,
                Games = games,
                Pages = (int)Math.Ceiling((double)search.Count / maxResults),
                Total = (int)search!.Count
            };
        }

        return new Products()
        {
            CurrentPage = page,
            Games = new List<Product>(),
            Pages = 0,
            Total = 0
        };
    }
}