using CosmicObserverAPI.Configuration;
using CosmicObserverAPI.Interfaces;
using Microsoft.Extensions.Options;

namespace CosmicObserverAPI.Services;

public class NasaApodService : INasaApodService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NasaApodService(HttpClient httpClient, IOptions<NasaApiOptions> options)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri("https://api.nasa.gov");

        _apiKey = options.Value.ApiKey;
    }

    public async Task<string?> GetApodJson() =>
        await _httpClient.GetStringAsync($"planetary/apod?api_key={_apiKey}");
}
