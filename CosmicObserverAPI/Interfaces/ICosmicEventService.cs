using CosmicObserverAPI.DTOs;

namespace CosmicObserverAPI.Interfaces;

public interface ICosmicEventService
{
    Task<bool> SaveApodAsync(NasaApodResponse todayResponse);
}
