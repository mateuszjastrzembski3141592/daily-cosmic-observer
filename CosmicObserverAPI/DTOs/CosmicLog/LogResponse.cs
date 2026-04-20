using CosmicObserverAPI.DTOs.CosmicTag;

namespace CosmicObserverAPI.DTOs.CosmicLog;

public class LogResponse
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required string Category { get; init; }
    public required DateTime CreatedAt { get; init; }
    public int? CosmicEventId { get; init; }
    public string? SourceUrl { get; init; }
    public List<TagResponse> Tags { get; init; } = [];
}
