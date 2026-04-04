using CosmicObserverAPI.DTOs;
using CosmicObserverAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmicObserverAPI.Controllers;

[Route("api/Apod")]
[ApiController]
public class ApodController : ControllerBase
{
    private readonly INasaApodService _apodService;
    private readonly ICosmicEventService _eventService;

    public ApodController(INasaApodService apodService, ICosmicEventService eventService)
    {
        _apodService = apodService;
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<NasaApodResponse>> GetApod([FromQuery] DateOnly? date)
    {
        var apodResult = await _apodService.GetApodAsync(date);

        if (apodResult == null)
        {
            return NotFound();
        }

        await _eventService.SaveApodAsync(apodResult);

        return Ok(apodResult);
    }

    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<NasaApodResponse>>> GetApodRange([FromQuery] DateOnly startDate, DateOnly? endDate)
    {
        var apodResults = await _apodService.GetApodRangeAsync(startDate, endDate);

        if (!apodResults.Any())
        {
            return NotFound();
        }

        await _eventService.SaveApodRangeAsync(apodResults);

        return Ok(apodResults);
    }
}
