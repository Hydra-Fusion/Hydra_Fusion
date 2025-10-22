using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Internal.Details;

#nullable disable
public class Proton
{
    [JsonPropertyName("bestReportedTier")] public string BestReportedTier { get; set; }
    [JsonPropertyName("confidence")] public string Confidence { get; set; }
    [JsonPropertyName("score")] public double Score { get; set; }
    [JsonPropertyName("tier")] public string Tier { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
    [JsonPropertyName("trendingTier")] public string TrendingTier { get; set; }
}