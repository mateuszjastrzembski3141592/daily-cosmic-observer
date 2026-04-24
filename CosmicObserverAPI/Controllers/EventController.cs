using CosmicObserverAPI.DTOs.CosmicEvent;
using CosmicObserverAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmicObserverAPI.Controllers;

[Route("api/Events")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly ICosmicEventService _eventService;

    public EventController(ICosmicEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetAllEvents()
    {
        var eventResults = await _eventService.GetAllEventsAsync();

        if (!eventResults.Any())
        {
            return NotFound();
        }

        return Ok(eventResults);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponse>> GetEventById([FromRoute] int id)
    {
        var eventResult = await _eventService.GetEventByIdAsync(id);

        if (eventResult is null)
        {
            return NotFound();
        }

        return Ok(eventResult);
    }

    [HttpGet("date")]
    public async Task<ActionResult<EventResponse>> GetEventByDate([FromQuery] DateOnly date)
    {
        var eventResult = await _eventService.GetEventByDateAsync(date);

        if (eventResult is null)
        {
            return NotFound();
        }

        return Ok(eventResult);
    }

    [HttpGet("dates")]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetEventsRange([FromQuery] DateOnly startDate, DateOnly endDate)
    {
        var eventResults = await _eventService.GetEventsRangeAsync(startDate, endDate);

        if (!eventResults.Any())
        {
            return NotFound();
        }

        return Ok(eventResults);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent([FromRoute] int id)
    {
        return await _eventService.DeleteEventAsync(id) ? NoContent() : NotFound();
    }
}
