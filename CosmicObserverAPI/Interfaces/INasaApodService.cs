using CosmicObserverAPI.DTOs;

namespace CosmicObserverAPI.Interfaces;

public interface INasaApodService
{
    Task<NasaApodResponse?> GetApodAsync(DateOnly? date);
}
