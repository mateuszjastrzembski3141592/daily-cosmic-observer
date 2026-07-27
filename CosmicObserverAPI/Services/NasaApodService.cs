using CosmicObserverAPI.Configuration;
using CosmicObserverAPI.DTOs.Apod;
using CosmicObserverAPI.Enums;
using CosmicObserverAPI.Errors;
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
        var error = ValidateDates(date, date);

        if (error is not null)
        {
            return Result<NasaApodResponse>.Failure(error);
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
        var error = ValidateDates(startDate, endDate);

        if (error is not null)
        {
            return Result<IEnumerable<NasaApodResponse>>.Failure(error);
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
            return Result<T>.Failure(ApodErrors.ExternalRequestRejectionError);
        }

        var apodData = await response.Content.ReadFromJsonAsync<T>();

        if (apodData is null)
        {
            return Result<T>.Failure(ApodErrors.EmptyResponseError);
        }

        return Result<T>.Success(apodData);
    }

    private static Error? ValidateDates(DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate is not null && endDate is not null && startDate > endDate)
        {
            return ApodErrors.DateRangeError;
        }

        DateOnly firstApod = new(1995, 06, 16);
        DateOnly presentDay = DateOnly.FromDateTime(DateTime.Today);

        if ((startDate is not null && startDate < firstApod)
            || (endDate is not null && endDate < firstApod))
        {
            return ApodErrors.PastDateError;
        }
        else if ((startDate is not null && startDate > presentDay)
            || (endDate is not null && endDate > presentDay))
        {
            return ApodErrors.FutureDateError;
        }

        return null;
    }
}
