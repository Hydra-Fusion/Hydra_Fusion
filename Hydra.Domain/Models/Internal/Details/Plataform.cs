using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Internal.Details;

public record Platform
{
    [JsonPropertyName("windows")] public bool Windows { get; set; }
    [JsonPropertyName("mac")] public bool Mac { get; set; }
    [JsonPropertyName("linux")] public bool Linux { get; set; }
}