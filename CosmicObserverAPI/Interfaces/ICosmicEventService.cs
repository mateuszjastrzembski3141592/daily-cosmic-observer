using CosmicObserverAPI.DTOs;

namespace CosmicObserverAPI.Interfaces;

public interface ICosmicEventService
{
    Task<bool> SaveTodayApodAsync(NasaApodResponse todayResponse);
}
