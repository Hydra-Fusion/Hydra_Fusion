using System.Text.Json.Serialization;

namespace Hydra.Domain.Models.Gog;

#nullable disable
public class GogProduct
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("features")]
    public List<Feature> Features { get; set; }

    [JsonPropertyName("screenshots")]
    public List<string> Screenshots { get; set; }

    [JsonPropertyName("releaseDate")]
    public string ReleaseDate { get; set; }

    [JsonPropertyName("storeReleaseDate")]
    public string StoreReleaseDate { get; set; }

    [JsonPropertyName("productType")]
    public string ProductType { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("coverHorizontal")]
    public string CoverHorizontal { get; set; }

    [JsonPropertyName("coverVertical")]
    public string CoverVertical { get; set; }

    [JsonPropertyName("developers")]
    public List<string> Developers { get; set; }

    [JsonPropertyName("publishers")]
    public List<string> Publishers { get; set; }

    [JsonPropertyName("operatingSystems")]
    public List<string> OperatingSystems { get; set; }

    [JsonPropertyName("productState")]
    public string ProductState { get; set; }

    [JsonPropertyName("genres")]
    public List<Genre> Genres { get; set; }

    [JsonPropertyName("tags")]
    public List<Tag> Tags { get; set; }

    [JsonPropertyName("reviewsRating")]
    public int? ReviewsRating { get; set; }

    [JsonPropertyName("editions")]
    public List<Edition> Editions { get; set; }

    [JsonPropertyName("ratings")]
    public List<object> Ratings { get; set; }

    [JsonPropertyName("storeLink")]
    public string StoreLink { get; set; }
    
    public class Genre
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("slug")]
        public string slug { get; set; }
    }

    public class Edition
    {
        [JsonPropertyName("id")]
        public int? id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("isRootEdition")]
        public bool? isRootEdition { get; set; }
    }
    
    public class Feature
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("slug")]
        public string slug { get; set; }
    }
    
    
    public class Tag
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("slug")]
        public string slug { get; set; }
    }
}







