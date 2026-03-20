namespace CosmicObserverAPI.Models;

public class CosmicTag
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public List<CosmicLog> Logs { get; set; } = []; // Many-to-Many relation with CosmicLog
}
