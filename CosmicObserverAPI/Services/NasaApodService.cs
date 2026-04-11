using CosmicObserverAPI.Configuration;
using CosmicObserverAPI.DTOs.Apod;
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

        _apiKey = string.IsNullOrWhiteSpace(options.Value.ApiKey) ? "DEMO_KEY" : options.Value.ApiKey;
    }

    public async Task<NasaApodResponse?> GetApodAsync(DateOnly? date)
    {
        string queryUrl = $"planetary/apod?api_key={_apiKey}";

        if (date is DateOnly d) 
        {
            queryUrl += $"&date={d:yyyy-MM-dd}";
        }

        return await _httpClient.GetFromJsonAsync<NasaApodResponse>(queryUrl);
    }

    public async Task<IEnumerable<NasaApodResponse>> GetApodRangeAsync(DateOnly startDate, DateOnly? endDate)
    {
        string queryUrl = $"planetary/apod?api_key={_apiKey}&start_date={startDate:yyyy-MM-dd}";

        if (endDate is DateOnly d)
        {
            queryUrl += $"&end_date={d:yyyy-MM-dd}";
        }

        return await _httpClient.GetFromJsonAsync<IEnumerable<NasaApodResponse>>(queryUrl) ?? [];
    }
}
