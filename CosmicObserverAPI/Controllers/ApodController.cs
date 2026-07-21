using CosmicObserverAPI.DTOs.Apod;
using CosmicObserverAPI.Extensions;
using CosmicObserverAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmicObserverAPI.Controllers;

[Route("api/Apod")]
[ApiController]
public class ApodController : ControllerBase
{
    private readonly INasaApodService _apodService;

    public ApodController(INasaApodService apodService)
    {
        _apodService = apodService;
    }

    [HttpGet]
    public async Task<ActionResult<NasaApodResponse>> GetApod([FromQuery] DateOnly? date)
    {
        var apodResult = await _apodService.GetApodAsync(date);

        return apodResult.Map<ActionResult<NasaApodResponse>>(
            onSuccess: value => Ok(value),
            onFailure: error => this.ToErrorCode(error)
        );
    }

    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<NasaApodResponse>>> GetApodRange([FromQuery] DateOnly startDate, DateOnly? endDate)
    {
        var apodResults = await _apodService.GetApodRangeAsync(startDate, endDate);

        return apodResults.Map<ActionResult<IEnumerable<NasaApodResponse>>>(
            onSuccess: value => Ok(value),
            onFailure: error => this.ToErrorCode(error)
        );
    }
}
