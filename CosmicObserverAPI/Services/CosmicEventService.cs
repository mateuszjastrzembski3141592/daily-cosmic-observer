using CosmicObserverAPI.Data;
using CosmicObserverAPI.DTOs;
using CosmicObserverAPI.Interfaces;
using CosmicObserverAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmicObserverAPI.Services;

public class CosmicEventService : ICosmicEventService
{
    private readonly CosmicDbContext _cosmicDbContext;

    public CosmicEventService(CosmicDbContext cosmicDbContext)
    {
        _cosmicDbContext = cosmicDbContext;
    }

    public async Task<bool> SaveApodAsync(NasaApodResponse todayResponse)
    {
        var db = _cosmicDbContext.CosmicEvents;

        if (await db.AnyAsync(e => e.Date == todayResponse.Date))
        {
            return false;
        }

        var todayCosmicEvent = new CosmicEvent()
        {
            Title = todayResponse.Title,
            Description = todayResponse.Description,
            Date = todayResponse.Date,
            ImageUrl = todayResponse.ImageUrl,
            SourceUrl = $"https://apod.nasa.gov/apod/ap{todayResponse.Date:yyMMdd}.html"
        };

        db.Add(todayCosmicEvent);
        await _cosmicDbContext.SaveChangesAsync();

        return true;
    }
}
