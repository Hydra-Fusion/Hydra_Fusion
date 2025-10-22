using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Internal.Details;

public record Screenshot(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("path_thumbnail")] string PathThumbnail,
    [property: JsonPropertyName("path_full")] string PathFull
);