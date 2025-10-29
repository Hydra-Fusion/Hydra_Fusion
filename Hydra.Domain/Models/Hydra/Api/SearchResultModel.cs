using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Hydra.Api;

public class SearchResultModel
{
    [JsonPropertyName("objectId")] public string? ObjectId { get; set; }
    
    [JsonPropertyName("title")] public string? Title { get; set; }
    
    [JsonPropertyName("shop")] public string? Shop { get; set; }
    
    [JsonPropertyName("genres")] public List<string>? Genres { get; set; }
    
    [JsonPropertyName("libraryImage")] public string? LibraryImage { get; set; }
    
    [JsonPropertyName("id")] public string? Id { get; set; }
}