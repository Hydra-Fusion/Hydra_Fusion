using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Internal.Details;

public record Recommendations(
    [property: JsonPropertyName("total")] int Total
);