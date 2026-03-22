namespace CosmicObserverAPI.Interfaces;

public interface INasaApodService
{
    Task<string?> GetApodJson();
}
