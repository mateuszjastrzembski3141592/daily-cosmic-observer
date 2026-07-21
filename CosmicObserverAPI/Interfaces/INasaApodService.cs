using CosmicObserverAPI.DTOs.Apod;
using CosmicObserverAPI.Shared;

namespace CosmicObserverAPI.Interfaces;

public interface INasaApodService
{
    Task<Result<NasaApodResponse>> GetApodAsync(DateOnly? date);

    Task<Result<IEnumerable<NasaApodResponse>>> GetApodRangeAsync(DateOnly startDate, DateOnly? endDate);
}
