using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Gog;

public class GogSearchResponse
{
    [JsonPropertyName("pages")] 
    public int Pages { get; set; }
    
    [JsonPropertyName("currentlyShownProductCount")]
    public int CurrentlyShownProductCount { get; set; }
    
    [JsonPropertyName("productCount")]
    public int Count { get; set; }
    
    [JsonPropertyName("products")]
    public List<GogProduct>? Products { get; set; }
}