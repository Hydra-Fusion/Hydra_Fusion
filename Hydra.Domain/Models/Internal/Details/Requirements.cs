using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Internal.Details;

public record Requirements
{
    [JsonPropertyName("minimum")] public string Minimum { get; set; }
    [JsonPropertyName("recommended")] public string Recommended { get; set; }
}