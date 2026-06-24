using CosmicObserverAPI.Data;
using CosmicObserverAPI.DTOs.Apod;
using CosmicObserverAPI.DTOs.CosmicEvent;
using CosmicObserverAPI.Extensions;
using CosmicObserverAPI.Interfaces;
using CosmicObserverAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CosmicObserverAPI.Services;

public class CosmicEventService : ICosmicEventService
{
    private readonly CosmicDbContext _cosmicDbContext;
    private readonly IMemoryCache _memoryCache;

    public CosmicEventService(CosmicDbContext cosmicDbContext, IMemoryCache memoryCache)
    {
        _cosmicDbContext = cosmicDbContext;
        _memoryCache = memoryCache;
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
        string cacheKey = $"event:{id}";

        var cachedEvent = await _memoryCache.GetOrCreateAsync(
            cacheKey,
            async cacheEntry =>
            {
                var db = _cosmicDbContext.CosmicEvents;
        
                var cEvent = await db
                .Where(ce => ce.Id == id)
                .Select(EventMappingExtensions.ToEventResponseExpression)
                .FirstOrDefaultAsync();

                if (cEvent is null)
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1);
                }
                else
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
                }

                return cEvent;
            });

        return cachedEvent;
    }

    public async Task<EventResponse?> GetEventByDateAsync(DateOnly date)
    {
        string cacheKey = $"eventDate:{date}";

        var cachedEvent = await _memoryCache.GetOrCreateAsync(
            cacheKey,
            async cacheEntry =>
            {
                var db = _cosmicDbContext.CosmicEvents;

                var cEvent = await db
                .Where(ce => ce.Date == date)
                .Select(EventMappingExtensions.ToEventResponseExpression)
                .FirstOrDefaultAsync();

                if (cEvent is null)
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1);
                }
                else
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
                }

                return cEvent;
            });

        return cachedEvent;
    }

    public async Task<IEnumerable<EventResponse>> GetEventsRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        async Task<List<EventResponse>> FetchEventsFromDbAsync()
        {
            var db = _cosmicDbContext.CosmicEvents;

            var cEvent = await db
                .Where(ce => ce.Date >= startDate && ce.Date <= endDate)
                .Select(EventMappingExtensions.ToEventResponseExpression)
                .ToListAsync();

            return cEvent;
        }

        int totalDays = endDate.DayNumber - startDate.DayNumber;

        if (totalDays > 30)
        {
            return await FetchEventsFromDbAsync();
        }

        string cacheKey = $"eventStartDate:{startDate}:eventEndDate:{endDate}";

        var cachedEvents = await _memoryCache.GetOrCreateAsync(
            cacheKey,
            async cacheEntries =>
            {
                var fetchResult = await FetchEventsFromDbAsync();

                if (fetchResult.Count < endDate.DayNumber - startDate.DayNumber + 1)
                {
                    cacheEntries.AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1);
                }
                else
                {
                    cacheEntries.SlidingExpiration = TimeSpan.FromMinutes(30);
                    cacheEntries.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
                }

                return fetchResult;
            });

        return cachedEvents ?? [];
    }

    public async Task<bool> SaveApodAsync(NasaApodResponse apodResponse)
    {
        var db = _cosmicDbContext.CosmicEvents;

        if (await db.AnyAsync(ce => ce.Date == apodResponse.Date))
        {
            return false;
        }

        var cosmicEvent = apodResponse.ToEventEntity();

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
            .Select(nar => nar.ToEventEntity());

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
