namespace Hydra.Domain.Models.Internal.Catalogue;

public class Products
{
    public int Total { get; set; }
    
    public int Pages { get; set; }
    
    public int CurrentPage { get; set; }
    
    public List<Product> Games { get; set; }
}