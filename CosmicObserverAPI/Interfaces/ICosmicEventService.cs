using CosmicObserverAPI.DTOs;

namespace CosmicObserverAPI.Interfaces;

public interface ICosmicEventService
{
    Task<bool> SaveApodAsync(NasaApodResponse apodResponse);

    Task<bool> SaveApodRangeAsync(IEnumerable<NasaApodResponse> apodResponses);
}
