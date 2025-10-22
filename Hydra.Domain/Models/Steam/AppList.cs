using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Steam;

#nullable disable
public record AppList
{
    [JsonPropertyName("applist")] public Apps Data { get; set; }
    
    public record Apps
    {
        [JsonPropertyName("apps")] public List<Apps> Items { get; set; }

        public record App
        {
            [JsonPropertyName("appid")] int Id { get; set; }
            
            [JsonPropertyName("name")] string Name { get; set; }
        }
    }
}