using CosmicObserverAPI.Data;
using CosmicObserverAPI.DTOs.Tags;
using CosmicObserverAPI.Interfaces;
using CosmicObserverAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmicObserverAPI.Services;

public class CosmicTagService : ICosmicTagService
{
    private readonly CosmicDbContext _cosmicDbContext;

    public CosmicTagService(CosmicDbContext cosmicDbContext)
    {
        _cosmicDbContext = cosmicDbContext;
    }

    public async Task<IEnumerable<TagResponse>> GetAllTagsAsync()
    {
        var db = _cosmicDbContext.CosmicTags;

        var tags = await db
            .AsNoTracking()
            .Select(ct => new TagResponse()
            {
                Id = ct.Id,
                Name = ct.Name
            })
            .ToListAsync();

        return tags;
    }

    public async Task<TagResponse?> GetTagByIdAsync(int id)
    {
        var db = _cosmicDbContext.CosmicTags;

        var tag = await db
            .Where(ct => ct.Id == id)
            .Select(ct => new TagResponse()
            {
                 Id = ct.Id,
                 Name = ct.Name
            })
            .FirstOrDefaultAsync();

        return tag;
    }

    public async Task<TagResponse?> CreateTagAsync(CreateTag newTag)
    {
        var db = _cosmicDbContext.CosmicTags;

        if (await db.AnyAsync(ct => ct.Name == newTag.Name))
        {
            return null;
        }

        var cosmicTag = new CosmicTag()
        {
            Name = newTag.Name
        };

        db.Add(cosmicTag);
        await _cosmicDbContext.SaveChangesAsync();

        var tag = new TagResponse()
        {
            Id = cosmicTag.Id,
            Name = cosmicTag.Name
        };

        return tag;
    }

    public async Task<bool> DeleteTagAsync(int id)
    {
        var db = _cosmicDbContext.CosmicTags;

        var tag = await db.FindAsync(id);

        if (tag is null)
        {
            return false;
        }

        db.Remove(tag);
        await _cosmicDbContext.SaveChangesAsync();

        return true;
    }
}
