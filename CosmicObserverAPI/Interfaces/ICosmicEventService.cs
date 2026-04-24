using CosmicObserverAPI.DTOs.Apod;
using CosmicObserverAPI.DTOs.CosmicEvent;

namespace CosmicObserverAPI.Interfaces;

public interface ICosmicEventService
{
    Task<IEnumerable<EventResponse>> GetAllEventsAsync();

    Task<EventResponse?> GetEventByIdAsync(int id);

    Task<EventResponse?> GetEventByDateAsync(DateOnly date);

    Task<IEnumerable<EventResponse>> GetEventsRangeAsync(DateOnly startDate, DateOnly endDate);

    Task<bool> SaveApodAsync(NasaApodResponse apodResponse);

    Task<bool> SaveApodRangeAsync(IEnumerable<NasaApodResponse> apodResponses);

    Task<bool> DeleteEventAsync(int id);
}
