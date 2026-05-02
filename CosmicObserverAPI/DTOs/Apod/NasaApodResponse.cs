using System.Text.Json.Serialization;

namespace CosmicObserverAPI.DTOs.Apod;

public class NasaApodResponse
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }
    [JsonPropertyName("explanation")]
    public required string Description { get; set; }
    [JsonPropertyName("date")]
    public required DateOnly Date { get; set; }
    [JsonPropertyName("url")]
    public required string ImageUrl { get; set; } // Note: can contain video url instead of image url
}
