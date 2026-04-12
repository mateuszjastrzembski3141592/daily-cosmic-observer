namespace CosmicObserverAPI.Models;

public class CosmicEvent
{
    public int Id { get; init; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required string ImageUrl { get; set; }
    public required string SourceUrl { get; set; }
    public ICollection<CosmicLog> Logs { get; } = [];
}
