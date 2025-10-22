namespace Hydra.Domain.Models.Hydra.Database;

public class Links
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public List<string> Uris { get; set; }
    public string Size { get; set; }
    
    public DateTime DateUpdated { get; set; }
    public DateTime DateCreated { get; set; }
    
    
    public virtual int FontId { get; set; }
    public virtual Fonts Fonts { get; set; }
}