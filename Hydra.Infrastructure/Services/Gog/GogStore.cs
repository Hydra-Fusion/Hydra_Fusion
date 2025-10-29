using Hydra.Domain.Models.Internal.Catalogue;
using Hydra.Domain.Models.Internal.Details;
using Hydra.Domain.Models.Lucene;
using Hydra.Infrastructure.Interfaces.Refit;
using Hydra.Infrastructure.Services.Lucene;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers.Classic;
using Microsoft.Extensions.Caching.Memory;
using QueryParser = Lucene.Net.QueryParsers.Surround.Parser.QueryParser;

namespace Hydra.Infrastructure.Services.Gog;

public class GogStore
{
    private readonly IGogStore _store;
    private readonly LuceneStore _luceneStore;
    private IMemoryCache _cache;
    
    public GogStore(IGogStore store, IMemoryCache cache)
    {
        _store = store;
        _cache = cache;
        _luceneStore = new LuceneStore("Gog", "Index/Stores/Gog");
    }

    public async Task<Products> SearchAsync(string query, int page, int maxResults = 20)
    {
        if (page <= 0)
            throw new ArgumentException("page must be greater than zero");
        
        List<Product> games = new();
        
        int pagesPerApiCall = 100 / maxResults;
        
        int blockIndex = (page - 1) / pagesPerApiCall;
        
        int pageCurrentApiPage = (blockIndex * pagesPerApiCall) + 1;
        
        if (_cache.TryGetValue($"Gog-{QueryParser.Parse(query)}-{pageCurrentApiPage}", out int count))
        {
            var search = _luceneStore.Search(query, page, maxResults);
            return search.ConvertToProducts(count, maxResults);
        }
        
        var responseGog = await _store.SearchAsync(
            query, 
            blockIndex, 
            100);

        if (responseGog.IsSuccessStatusCode)
        {
            var search = responseGog.Content;
            
            var products = search.Products?.Select(x => new Product
            {
                Name = x.Title,

                Tags = x.Tags
                    .Select(x => x.name)
                    .ToList(),

                Developers = x.Developers,
                Publishers = x.Publishers,

                Cover = x.CoverHorizontal,
                Id_Gog = x.Id,
                Store = Store.Gog,
                Platform = new Platform
                {
                    Windows = x.OperatingSystems.Contains("windows"),
                    Linux = x.OperatingSystems.Contains("linux"),
                    Mac = x.OperatingSystems.Contains("osx")
                },
                Release = x.ReleaseDate
            }).ToList() ?? new List<Product>();
            
            await _luceneStore.AddOrUpdateBulkAsync(products
                .Select(x => (GameDocument)x)
                .ToList(), document => (Document)document);
            
            _cache.Set($"Gog-{query.ToLower().Trim()}-{pageCurrentApiPage}", search!.Count, DateTimeOffset.UtcNow.AddMinutes(30));
            
            return new Products
            {
                Pages = (int)Math.Ceiling((double)search.Count / maxResults),
                Total = search.Count,
                CurrentPage = page,
                Games = products
                    .ToList()
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