namespace CosmicObserverAPI.DTOs.CosmicLog;

public class CreateLog
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Category { get; set; }
    public int? CosmicEventId { get; set; }
    public string? SourceUrl { get; set; }
    public List<string> Tags { get; set; } = [];
}
