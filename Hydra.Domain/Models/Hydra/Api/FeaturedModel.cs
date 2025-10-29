using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Hydra.Api;

public class FeaturedModel
{
    [JsonPropertyName("shop")] public string? Shop { get; set; }
    
    [JsonPropertyName("description")] public string? Description { get; set; }
    
    [JsonPropertyName("title")] public string? Title { get; set; }
    
    [JsonPropertyName("objectId")] public string? Id { get; set; }
    
    [JsonPropertyName("libraryHeroImageUrl")] public string? LibraryHeroImage { get; set; }
    
    [JsonPropertyName("logoImageUrl")] public string? LogoImage { get; set; }
    
    [JsonPropertyName("uri")] public string? Uri { get; set; }
}