using Hydra.Domain.Models.Internal.Catalogue;
using Hydra.Domain.Models.Internal.Details;
using Hydra.Domain.Models.Lucene;
using Hydra.Infrastructure.Interfaces.Refit;
using Hydra.Infrastructure.Services.Lucene;
using Lucene.Net.Documents;

namespace Hydra.Infrastructure.Services.Gog;

public class GogStore
{
    private readonly IGogStore _store;
    private readonly LuceneStore _luceneStore;

    public GogStore(IGogStore store)
    {
        _store = store;
        _luceneStore = new LuceneStore("Gog", "Index/Stores/Gog");
    }

    public async Task<Products> SearchAsync(string query, int page)
    {
        if (page <= 0)
            throw new ArgumentException("page must be greater than zero");
        
        var results = await _store.SearchAsync(
            query, 
            page % 2 == 0 ? page - 1 : page, 
            100);

        if (results.IsSuccessStatusCode || results.Content == null)
        {
            var search = _luceneStore.Search(query, page);
            
            var games = search?.Games?.Select(x => new Product
            {
                Name = x.Title,
                Cover = x.Cover,
                Description = x.Description,
                Categories = x.Categories?.Split(',').ToList(),
                Tags = x.Tags?.Split(',').ToList(),
                Developers = x.Developers?.Split(',').ToList(),
                Publishers = x.Publishers?.Split(',').ToList()
            }).ToList() ?? new();
            
            
            return new Products()
            {
                CurrentPage = page,
                Games = games,
                Pages = search.TotalPages,
                Total = search.TotalResults
            };
        }
            
        
        var gogResponse = results.Content;

        var products = gogResponse.Products?.Select(x => new Product
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
        
        
        return new Products
        {
            Pages = gogResponse.Pages,
            Total = gogResponse.Count,
            CurrentPage = page,
            Games = products
                .ToList()
        };
    }
}