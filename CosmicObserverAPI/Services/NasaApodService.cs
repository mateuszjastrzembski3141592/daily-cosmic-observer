using CosmicObserverAPI.Configuration;
using CosmicObserverAPI.DTOs.Apod;
using CosmicObserverAPI.Enums;
using CosmicObserverAPI.Interfaces;
using CosmicObserverAPI.Shared;
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

    public async Task<Result<NasaApodResponse>> GetApodAsync(DateOnly? date)
    {
        DateOnly firstApod = new(1995, 06, 16);

        if (date < firstApod)
        {
            return Result<NasaApodResponse>.Failure(new Error("Date can't be before 16-06-1995", "TooEarlyDateError", ErrorType.Validation));
        }
        else if (date > DateOnly.FromDateTime(DateTime.Today))
        {
            return Result<NasaApodResponse>.Failure(new Error("Date can't be a future date", "FutureDateError", ErrorType.Validation));
        }

        string queryUrl = $"planetary/apod?api_key={_apiKey}";

        if (date is DateOnly d) 
        {
            queryUrl += $"&date={d:yyyy-MM-dd}";
        }

        return await ToApodResult<NasaApodResponse>(queryUrl);
    }

    public async Task<Result<IEnumerable<NasaApodResponse>>> GetApodRangeAsync(DateOnly startDate, DateOnly? endDate)
    {
        DateOnly firstApod = new(1995, 06, 16);

        if (startDate < firstApod)
        {
            return Result<IEnumerable<NasaApodResponse>>.Failure(new Error("Date can't be before 16-06-1995", "TooEarlyDateError", ErrorType.Validation));
        }
        else if(endDate > DateOnly.FromDateTime(DateTime.Today))
        {
            return Result<IEnumerable<NasaApodResponse>>.Failure(new Error("Date can't be a future date", "FutureDateError", ErrorType.Validation));
        }

        string queryUrl = $"planetary/apod?api_key={_apiKey}&start_date={startDate:yyyy-MM-dd}";

        if (endDate is DateOnly d)
        {
            queryUrl += $"&end_date={d:yyyy-MM-dd}";
        }

        return await ToApodResult<IEnumerable<NasaApodResponse>>(queryUrl);
    }

    private async Task<Result<T>> ToApodResult<T>(string queryUrl)
    {
        var response = await _httpClient.GetAsync(queryUrl);

        if (!response.IsSuccessStatusCode)
        {
            return Result<T>.Failure(new Error("Request rejected by APOD API", "ExternalRequestRejectionError", ErrorType.Failure));
        }

        var apodData = await response.Content.ReadFromJsonAsync<T>();

        if (apodData is null)
        {
            return Result<T>.Failure(new Error("The response is empty", "NullResponseError", ErrorType.Failure));
        }

        return Result<T>.Success(apodData);
    }
}
