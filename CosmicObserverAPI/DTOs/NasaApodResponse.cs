using System.Text.Json.Serialization;

namespace CosmicObserverAPI.DTOs;

public class NasaApodResponse
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }
    [JsonPropertyName("explanation")]
    public required string Description { get; set; }
    [JsonPropertyName("date")]
    public required DateOnly Date { get; set; }
    [JsonPropertyName("hdurl")]
    public required string ImageUrl { get; set; } // HD version
}
