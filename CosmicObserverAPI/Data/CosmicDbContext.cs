using Microsoft.EntityFrameworkCore;
using CosmicObserverAPI.Models;

namespace CosmicObserverAPI.Data;

public class CosmicDbContext : DbContext
{
    public DbSet<CosmicEvent> CosmicEvents { get; set; }
    public DbSet<CosmicLog> CosmicLogs { get; set; }
    public DbSet<CosmicTag> CosmicTags { get; set; }

    public CosmicDbContext(DbContextOptions<CosmicDbContext> options)
        : base(options)
    {

    }
}
