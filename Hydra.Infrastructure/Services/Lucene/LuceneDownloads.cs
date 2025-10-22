using FuzzySharp;
using Hydra.Domain.Models.Lucene;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Util;
using Newtonsoft.Json;

namespace Hydra.Infrastructure.Services.Lucene;

public class LuceneDownloads : LuceneBased<DownloadDocument>
{
    public LuceneDownloads() : base("Download", "Index/Download") { }
    
    public IEnumerable<DownloadDocument> GetLinksAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            yield break;
        
        var reader = _writer.GetReader(false);
        var search = new IndexSearcher(reader);

        var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
        var parser = new QueryParser(LuceneVersion.LUCENE_48, "Title", analyzer);


        var query = parser.Parse(QueryParser.Escape(name));

        var hits = search.Search(query, 100).ScoreDocs;

        foreach (var hit in hits)
        {
            var doc = search.Doc(hit.Doc);
            var title = doc.Get("Title");

            if (string.IsNullOrEmpty(title))
                continue;

            var dateOk = DateTime.TryParse(doc.Get("UploadDate"), out var time);

            var normalizedTitle = title.Trim().ToLowerInvariant();
            var normalizedName = name.Trim().ToLowerInvariant();
            
            var ratio = Fuzz.TokenSetRatio(normalizedTitle, normalizedName);
            var partial = Fuzz.PartialRatio(normalizedTitle, normalizedName);
            var combined = (ratio * 0.7 + partial * 0.3);

            if (combined < 60)
                continue;

            yield return new DownloadDocument(
                title,
                doc.Get("Source"),
                dateOk ? time : DateTime.UtcNow)
            {
                Uris = JsonConvert.DeserializeObject<List<string>>(doc.Get("Uris")),
                Size = doc.Get("Size"),
            };
        }
    }



    
    public IEnumerable<string> GetAllSources()
    {
        using var reader = _writer.GetReader(false);
        var sources = new HashSet<string>();

        var terms = MultiFields.GetTerms(reader, "Source");
        if (terms == null) return sources;

        var termsEnum = terms.GetEnumerator();
        BytesRef term;
        
        while ((term = termsEnum.Next()) != null)
            sources.Add(term.Utf8ToString());

        return sources;
    }
}