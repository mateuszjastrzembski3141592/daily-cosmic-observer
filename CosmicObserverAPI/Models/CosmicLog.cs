namespace CosmicObserverAPI.Models;

public class CosmicLog
{
    public int Id { get; init; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Category { get; set; }
    public required DateTime CreatedAt { get; set; }
    public int? CosmicEventId { get; set; }
    public string? SourceUrl { get; set; }
    public List<CosmicTag> Tags { get; set; } = []; // Many-to-Many relation with cosmic Tag
    public CosmicEvent? CosmicEvent { get; set; }   // EF Core will perform SQL JOIN when .Include() is used later
}
