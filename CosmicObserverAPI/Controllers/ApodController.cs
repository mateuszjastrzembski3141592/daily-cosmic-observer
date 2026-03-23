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
    public async Task<ActionResult<string>> GetJsonString()
    {
        var jsonString = await _service.GetApodJson();

        if (jsonString == null)
        {
            return NotFound();
        }

        return jsonString;
    }
}
