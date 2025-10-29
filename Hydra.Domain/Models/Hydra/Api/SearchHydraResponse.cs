using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Hydra.Api;

public class SearchHydraResponse
{
    [JsonPropertyName("count")] public uint Count { get; set; }
    
    [JsonPropertyName("edges")] public SearchResultModel[] Edges { get; set; }
}