namespace Hydra.Domain.Models.Hydra.Database;

public class Fonts
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public virtual ICollection<Links> Links { get; set; }
}