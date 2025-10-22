using System.Text.Json.Serialization;
using Hydra.Domain.Models.Lucene;

namespace Hydra.Domain.Models.Hydra;

public class DownloadLink
{
     [JsonPropertyName("title")] 
     public string? Title { get; set; }
     
     [JsonPropertyName("fileSize")]
     public string? Size { get; set; }
     
     [JsonPropertyName("uris")]
     public List<string>? Uris { get; set; }
     
     [JsonPropertyName("uploadDate")]
     public string? UploadDate { get; set; }

     public DownloadDocument ToDownloadDocument(string source)
     {
          return new DownloadDocument(Title, source, DateTime.TryParse(UploadDate, out var dt) ? dt : DateTime.UtcNow)
          {
               Size = Size,
               Uris = Uris
          };
     }
}