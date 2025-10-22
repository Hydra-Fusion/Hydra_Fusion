using Hydra.Domain.Models.Lucene;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Util;

namespace Hydra.Infrastructure.Services.Lucene;

public class LuceneStore : LuceneBased<GameDocument>
{
    public LuceneStore(string name, string basePath) : base(name, basePath) { }

    public CatalogDocument Search(string query, int page, int maxResults = 50)
    {
        using var reader = _writer.GetReader(applyAllDeletes: true);
        var searcher = new IndexSearcher(reader);
        
        var result = InternalSearch(query, page, maxResults);

        var documents = result.ScoreDocs.Select(x =>
        {
            var doc = searcher.Doc(x.Doc);
            
            return new GameDocument(doc.Get("Id"))
            {
                Title = doc.Get("Title"),
                Tags = doc.Get("Tags"),
                Categories = doc.Get("Categories"),
                Description = doc.Get("Description"),
                Languages = doc.Get("Languages"),
                Cover = doc.Get("Cover"),
                Developers = doc.Get("Developers"),
                Publishers = doc.Get("Publishers"),
                RequiredAge = int.TryParse(doc.Get("RequiredAge"), out var age) ? age : 0
            };
        }).ToList();
        
        
        return new()
        {
            Source = Name,
            Page = page,
            Games = documents,
            TotalResults = result.TotalHits,
            TotalPages = (int)Math.Ceiling(result.TotalHits / (double)maxResults)
        };
    }
    
    private TopDocs InternalSearch(string query, int page, int maxResults)
    {
        int start = (page - 1) * maxResults;
        
        var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
        var fields = new[] { "Title", "Tags", "Categories" };

        var parser = new MultiFieldQueryParser(LuceneVersion.LUCENE_48, fields, analyzer)
        {
            DefaultOperator = Operator.OR
        };

        var escapedQuery = QueryParser.Escape(query.ToLowerInvariant());
        var wildcardQuery = new BooleanQuery();

        foreach (var field in fields)
        {
            var termQuery = new WildcardQuery(new Term(field, $"*{escapedQuery}*"));
            wildcardQuery.Add(termQuery, Occur.SHOULD);
        }

        using var reader = _writer.GetReader(applyAllDeletes: true);
        var searcher = new IndexSearcher(reader);
        
        ScoreDoc? lastDoc = null;

        if (start > 0)
        {
            var prevTopDocs = searcher.Search(wildcardQuery, start);
            if (prevTopDocs.ScoreDocs.Length > 0)
                lastDoc = prevTopDocs.ScoreDocs.Last();
        }
        
        return searcher.SearchAfter(lastDoc, wildcardQuery, maxResults);
    }


    public IEnumerable<GameDocument> AdvancedSearch(
        string? searchTerm = null,
        List<string>? tagsFilter = null,
        List<string>? categoriesFilter = null,
        int? minAge = null,
        int? maxAge = null,
        int maxResults = 50)
    {
        var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
        var queries = new List<Query>();
        
        var fuzzyFields = new[] { "Title", "Developers", "Publishers", "Categories", "Tags" };
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var parser = new MultiFieldQueryParser(LuceneVersion.LUCENE_48, fuzzyFields, analyzer)
            {
                FuzzyMinSim = 0.4f,
                DefaultOperator = Operator.OR
            };

            var fuzzyQuery = parser.Parse(searchTerm + "~4");
            queries.Add(fuzzyQuery);
        }
        
        if (tagsFilter != null && tagsFilter.Count > 0)
        {
            foreach (var tag in tagsFilter)
            {
                queries.Add(new TermQuery(new Term("Tags", tag)));
            }
        }
        
        if (categoriesFilter != null && categoriesFilter.Count > 0)
        {
            foreach (var cat in categoriesFilter)
            {
                queries.Add(new TermQuery(new Term("Categories", cat)));
            }
        }
        
        if (minAge.HasValue || maxAge.HasValue)
        {
            int lower = minAge ?? 0;
            int upper = maxAge ?? int.MaxValue;
            var rangeQuery = NumericRangeQuery.NewInt32Range("RequiredAge", lower, upper, true, true);
            queries.Add(rangeQuery);
        }

        Query finalQuery;
        
        if (queries.Count == 1)
            finalQuery = queries[0];
        else
        {
            var booleanQuery = new BooleanQuery();
            foreach (var q in queries)
            {
                booleanQuery.Add(q, Occur.MUST);
            }
            finalQuery = booleanQuery;
        }
        
        using var reader = _writer.GetReader(applyAllDeletes: true);
        var searcher = new IndexSearcher(reader);
        var hits = searcher.Search(finalQuery, maxResults).ScoreDocs;

        foreach (var hit in hits)
        {
            var doc = searcher.Doc(hit.Doc);

            yield return new GameDocument(doc.Get("Id"))
            {
                Title = doc.Get("Title"),
                Tags = doc.Get("Tags"),
                Categories = doc.Get("Categories"),
                Description = doc.Get("Description"),
                Cover = doc.Get("Cover"),
                Languages = doc.Get("Languages"),
                Developers = doc.Get("Developers"),
                Publishers = doc.Get("Publishers"),
                RequiredAge = int.TryParse(doc.Get("RequiredAge"), out var age) ? age : 0
            };
        }
    }

    
}