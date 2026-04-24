using CosmicObserverAPI.Data;
using CosmicObserverAPI.DTOs.Apod;
using CosmicObserverAPI.DTOs.CosmicEvent;
using CosmicObserverAPI.Extensions;
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

    public async Task<IEnumerable<EventResponse>> GetAllEventsAsync()
    {
        var db = _cosmicDbContext.CosmicEvents;

        var cEvents = await db
            .Select(EventMappingExtensions.ToEventResponseExpression)
            .ToListAsync();

        return cEvents;
    }

    public async Task<EventResponse?> GetEventByIdAsync(int id)
    {
        var db = _cosmicDbContext.CosmicEvents;

        var cEvent = await db
            .Where(ce => ce.Id == id)
            .Select(EventMappingExtensions.ToEventResponseExpression)
            .FirstOrDefaultAsync();

        return cEvent;
    }

    public async Task<EventResponse?> GetEventByDateAsync(DateOnly date)
    {
        var db = _cosmicDbContext.CosmicEvents;

        var cEvent = await db
            .Where(ce => ce.Date == date)
            .Select(EventMappingExtensions.ToEventResponseExpression)
            .FirstOrDefaultAsync();

        return cEvent;
    }

    public async Task<IEnumerable<EventResponse>> GetEventsRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        var db = _cosmicDbContext.CosmicEvents;

        var cEvent = await db
            .Where(ce => ce.Date >= startDate && ce.Date <= endDate)
            .Select(EventMappingExtensions.ToEventResponseExpression)
            .ToListAsync();

        return cEvent;
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

    public async Task<bool> DeleteEventAsync(int id)
    {
        var db = _cosmicDbContext.CosmicEvents;

        var cEvent = await db.FindAsync(id);

        if (cEvent is null)
        {
            return false;
        }

        db.Remove(cEvent);
        await _cosmicDbContext.SaveChangesAsync();

        return true;
    }
}
