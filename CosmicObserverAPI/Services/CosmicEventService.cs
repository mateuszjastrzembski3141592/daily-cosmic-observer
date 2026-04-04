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

    public async Task<bool> SaveApodAsync(NasaApodResponse apodResponse)
    {
        var db = _cosmicDbContext.CosmicEvents;

        if (await db.AnyAsync(ce => ce.Date == apodResponse.Date))
        {
            return false;
        }

        var cosmicEvent = new CosmicEvent()
        {
            Title = apodResponse.Title,
            Description = apodResponse.Description,
            Date = apodResponse.Date,
            ImageUrl = apodResponse.ImageUrl,
            SourceUrl = $"https://apod.nasa.gov/apod/ap{apodResponse.Date:yyMMdd}.html"
        };

        db.Add(cosmicEvent);
        await _cosmicDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SaveApodRangeAsync(IEnumerable<NasaApodResponse> apodResponses)
    {
        var db = _cosmicDbContext.CosmicEvents;

        var dates = apodResponses
            .Select(nar => nar.Date)
            .ToList();

        var existingDates = await db
            .Where(ce => dates.Contains(ce.Date))
            .Select(ce => ce.Date)
            .ToListAsync();

        var filteredApods = apodResponses
            .Where(nar => !existingDates.Contains(nar.Date))
            .Select(nar => new CosmicEvent()
            {
                Title = nar.Title,
                Description = nar.Description,
                Date = nar.Date,
                ImageUrl = nar.ImageUrl,
                SourceUrl = $"https://apod.nasa.gov/apod/ap{nar.Date:yyMMdd}.html"
            });

        await db.AddRangeAsync(filteredApods);
        await _cosmicDbContext.SaveChangesAsync();

        return true;
    }
}
