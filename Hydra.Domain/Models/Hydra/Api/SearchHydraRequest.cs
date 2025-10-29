using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Hydra.Api;

public class SearchHydraRequest
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    
    [JsonPropertyName("tags")] public List<string>? Tags { get; set; }
    
    [JsonPropertyName("genres")] public List<string>? Genres { get; set; }
    
    [JsonPropertyName("publishers")] public List<string>? Publishers { get; set; }
    
    [JsonPropertyName("developers")] public List<string>? Developers { get; set; }

    [JsonPropertyName("take")] public int Take { get; set; } = 100;
    
    [JsonPropertyName("skip")] public int Skip { get; set; } = 0;
}