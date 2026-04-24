namespace CosmicObserverAPI.DTOs.CosmicEvent;

public class EventResponse
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required DateOnly Date { get; init; }
    public required string ImageUrl { get; init; }
    public required string SourceUrl { get; init; }
}
