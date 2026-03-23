using CosmicObserverAPI.DTOs;
using CosmicObserverAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmicObserverAPI.Controllers;

[Route("api/Apod")]
[ApiController]
public class ApodController : ControllerBase
{
    private readonly INasaApodService _service;

    public ApodController(INasaApodService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<NasaApodResponse>> GetTodayApod()
    {
        var apodResult = await _service.GetTodayApodAsync();

        if (apodResult == null)
        {
            return NotFound();
        }

        return apodResult;
    }
}
