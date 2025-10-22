using Hydra.Domain.Models.Internal.Details;
using Hydra.Domain.Models.Lucene;

namespace Hydra.Domain.Models.Internal.Catalogue;

#nullable disable
public class Product
{
    public int Id_Steam { get; set; }
    public string Id_Gog { get; set; }
    
    public string Description { get; set; }
    
    public List<string> Developers { get; set; }
    
    public List<string> Publishers { get; set; }
    
    public Store Store { get; set; }
    
    public string Name { get; set; }
    
    public string Cover { get; set; }
    public List<string> Categories { get; set; }
    public List<string> Tags { get; set; }
    
    public Platform Platform { get; set; }
    
    public string Release { get; set; }
    
    public static implicit operator GameDocument(Product product)
    {
        string id = product.Store switch
        {
            Store.Steam => product.Id_Steam.ToString(),
            Store.Gog => product.Id_Gog,
            _ => throw new NotSupportedException("Unknown Store")
        };
        
        return new GameDocument(id)
        {
            Title = product.Name,
            Cover = product.Cover,
            Description = product.Description,
            RequiredAge = 0,
            Categories = string.Join(',',  product.Categories ?? new()),
            Tags = string.Join(',', product.Tags ?? new()),
            Developers = string.Join(',',  product.Developers ?? new()),
            Publishers = string.Join(',', product.Publishers ?? new())
        };
    }
}

public enum Store
{
    Steam,
    Gog,
    Out
}