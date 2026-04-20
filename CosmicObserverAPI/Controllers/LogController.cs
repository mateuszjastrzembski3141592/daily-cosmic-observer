using CosmicObserverAPI.DTOs.CosmicLog;
using CosmicObserverAPI.Enums;
using CosmicObserverAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmicObserverAPI.Controllers;

[Route("api/Logs")]
[ApiController]
public class LogController : ControllerBase
{
    private readonly ICosmicLogService _logService;

    public LogController(ICosmicLogService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogResponse>>> GetAllLogs()
    {
        var logResults = await _logService.GetAllLogsAsync();

        if (!logResults.Any())
        {
            return NotFound();
        }

        return Ok(logResults);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LogResponse>> GetLogById([FromRoute] int id)
    {
        var logResult = await _logService.GetLogByIdAsync(id);

        if (logResult is null)
        {
            return NotFound();
        }

        return Ok(logResult);
    }

    [HttpGet("Categories")]
    public async Task<ActionResult<IEnumerable<LogResponse>>> GetLogsByCategory([FromQuery(Name = "category")] LogCategory[] categories)
    {
        var logResults = await _logService.GetLogsByCategoryAsync(categories);

        if (!logResults.Any())
        {
            return NotFound();
        }

        return Ok(logResults);
    }

    [HttpGet("Tags")]
    public async Task<ActionResult<IEnumerable<LogResponse>>> GetLogsByTagsAsync([FromQuery(Name = "tag")] string[] tags)
    {
        var logResults = await _logService.GetLogsByTagsAsync(tags);

        if (!logResults.Any())
        {
            return NotFound();
        }

        return Ok(logResults);
    }

    [HttpPost]
    public async Task<ActionResult<LogResponse>> CreateLog([FromBody] CreateLog newLog)
    {
        var logResult = await _logService.CreateLogAsync(newLog);

        if (logResult is null)
        {
            return Conflict();
        }

        return CreatedAtAction(nameof(GetLogById), new { id = logResult.Id }, logResult);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LogResponse>> UpdateLog([FromBody] CreateLog newLog, [FromRoute] int id)
    {
        var logResult = await _logService.UpdateLogAsync(newLog, id);

        if (logResult is null)
        {
            return Conflict();
        }

        return Ok(logResult);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLog([FromRoute] int id)
    {
        return await _logService.DeleteLogAsync(id) ? NoContent() : NotFound();
    }
}
