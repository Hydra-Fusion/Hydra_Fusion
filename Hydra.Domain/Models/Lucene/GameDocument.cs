using Hydra.Domain.Models.Internal.Catalogue;
using Lucene.Net.Documents;

namespace Hydra.Domain.Models.Lucene;

public record CatalogDocument
{
    public string? Source { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<GameDocument> Games { get; set; } = Enumerable.Empty<GameDocument>();

    public Products ConvertToProducts(int totalResults, int maxResults)
    {
        return new Products()
        {
            CurrentPage = Page,
            Games = ConvertDocumentGames(),
            Pages = (int)Math.Ceiling((double)totalResults / maxResults),
            Total = totalResults
        };
    }
    
    private List<Product> ConvertDocumentGames()
        => Games?.Select(x => new Product
        {
            Name = x.Title,
            Cover = x.Cover,
            Description = x.Description,
            Categories = x.Categories?.Split(',').ToList(),
            Tags = x.Tags?.Split(',').ToList(),
            Developers = x.Developers?.Split(',').ToList(),
            Publishers = x.Publishers?.Split(',').ToList()
        }).ToList() ?? new();
}

public record GameDocument(string Id) : DocumentBase(Id), IDisposable
{
    public string? Title { get; set; }
    public string? Cover { get; set; }
    public int RequiredAge { get; set; }
    public string? Description { get; set; }
    public string? Languages { get; set; }
    public string? Developers { get; set; }
    public string? Publishers { get; set; }
    public string? Categories { get; set; }
    public string? Tags { get; set; }
    
    public void Dispose() { }
    
    public static explicit operator Document(GameDocument doc)
    {
        var document = new Document
        {
            new StringField("Id", doc.Id, Field.Store.YES),
            new TextField("Title", doc.Title ?? string.Empty, Field.Store.YES),
            new StringField("Cover", doc.Cover ?? string.Empty, Field.Store.YES),
            new Int32Field("RequiredAge", doc.RequiredAge, Field.Store.YES),
            new TextField("Description", doc.Description ?? string.Empty, Field.Store.YES),
            new TextField("Languages", doc.Languages ?? string.Empty, Field.Store.YES),
            new TextField("Developers", doc.Developers ?? string.Empty, Field.Store.YES),
            new TextField("Publishers", doc.Publishers ?? string.Empty, Field.Store.YES),
            new TextField("Categories", doc.Categories ?? string.Empty, Field.Store.YES),
            new TextField("Tags", doc.Tags ?? string.Empty, Field.Store.YES)
        };

        return document;
    }
}