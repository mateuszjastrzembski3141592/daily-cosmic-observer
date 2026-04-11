using CosmicObserverAPI.DTOs.Tags;
using CosmicObserverAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CosmicObserverAPI.Controllers;

[Route("api/Tags")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ICosmicTagService _tagService;

    public TagController(ICosmicTagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagResponse>>> GetAllTags()
    {
        var tagResult = await _tagService.GetAllTagsAsync();

        if (!tagResult.Any())
        {
            return NotFound();
        }

        return Ok(tagResult);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagResponse>> GetTagById([FromRoute] int id)
    {
        var tagResult = await _tagService.GetTagByIdAsync(id);

        if (tagResult is null)
        {
            return NotFound();
        }

        return Ok(tagResult);
    }

    [HttpPost]
    public async Task<ActionResult<TagResponse>> CreateTag([FromBody] CreateTag newTag)
    {
        var tagResult = await _tagService.CreateTagAsync(newTag);

        if (tagResult is null)
        {
            return Conflict();
        }

        return CreatedAtAction(nameof(GetTagById), new { id = tagResult.Id }, tagResult);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TagResponse>> UpdateTag([FromBody] CreateTag newTag, [FromRoute] int id)
    {
        var tagResult = await _tagService.UpdateTagAsync(newTag, id);

        if (tagResult is null)
        {
            return Conflict();
        }

        return Ok(tagResult);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag([FromRoute] int id)
    {
        return await _tagService.DeleteTagAsync(id) ? NoContent() : NotFound();
    }
}
