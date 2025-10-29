using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Hydra.Api;

public class HotGameModel
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    
    [JsonPropertyName("shop")] public string? Shop { get; set; }
    
    [JsonPropertyName("objectId")] public string? ObjectId { get; set; }
    
    [JsonPropertyName("libraryImageUrl")] public string? LibreryImageUrl { get; set; }
}