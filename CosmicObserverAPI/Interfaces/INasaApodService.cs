using CosmicObserverAPI.DTOs;

namespace CosmicObserverAPI.Interfaces;

public interface INasaApodService
{
    Task<NasaApodResponse?> GetTodayApodAsync();
}
