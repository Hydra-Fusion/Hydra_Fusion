using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Hydra;

public class HydraLinks
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("downloads")]
    public List<DownloadLink>? Downloads { get; set; }
}