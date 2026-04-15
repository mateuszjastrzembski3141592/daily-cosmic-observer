using CosmicObserverAPI.Data;
using CosmicObserverAPI.DTOs.CosmicLog;
using CosmicObserverAPI.DTOs.CosmicTag;
using CosmicObserverAPI.Enums;
using CosmicObserverAPI.Interfaces;
using CosmicObserverAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmicObserverAPI.Services;

public class CosmicLogService : ICosmicLogService
{
    private readonly CosmicDbContext _cosmicDbContext;

    public CosmicLogService(CosmicDbContext cosmicDbContext)
    {
        _cosmicDbContext = cosmicDbContext;
    }

    public async Task<IEnumerable<LogResponse>> GetAllLogsAsync()
    {
        var db = _cosmicDbContext.CosmicLogs;

        var logs = await db
            .Select(cl => new LogResponse()
            {
                Id = cl.Id,
                Title = cl.Title,
                Content = cl.Content,
                Category = cl.Category,
                CreatedAt = cl.CreatedAt,
                CosmicEventId = cl.CosmicEventId,
                SourceUrl = cl.SourceUrl,
                Tags = cl.Tags
                    .Select(ct => new TagResponse()
                    {
                        Id = ct.Id,
                        Name = ct.Name
                    })
                    .ToList()
            })
            .ToListAsync();

        return logs;
    }

    public async Task<LogResponse?> GetLogByIdAsync(int id)
    {
        var db = _cosmicDbContext.CosmicLogs;

        var log = await db
            .Where(cl => cl.Id == id)
            .Select(cl => new LogResponse()
            {
                Id = cl.Id,
                Title = cl.Title,
                Content = cl.Content,
                Category = cl.Category,
                CreatedAt = cl.CreatedAt,
                CosmicEventId = cl.CosmicEventId,
                SourceUrl = cl.SourceUrl,
                Tags = cl.Tags
                    .Select(ct => new TagResponse()
                    {
                        Id = ct.Id,
                        Name = ct.Name
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return log;
    }

    public async Task<IEnumerable<LogResponse>> GetLogsByCategoryAsync(LogCategory[] categories)
    {
        var db = _cosmicDbContext.CosmicLogs;

        var categoryList = categories
            .Select(lc => lc.ToString())
            .ToList();

        var logs = await db
            .Where(cl => categoryList.Contains(cl.Category))
            .Select(cl => new LogResponse()
            {
                Id = cl.Id,
                Title = cl.Title,
                Content = cl.Content,
                Category = cl.Category,
                CreatedAt = cl.CreatedAt,
                CosmicEventId = cl.CosmicEventId,
                SourceUrl = cl.SourceUrl,
                Tags = cl.Tags
                    .Select(ct => new TagResponse()
                    {
                        Id = ct.Id,
                        Name = ct.Name
                    })
                    .ToList()
            })
            .ToListAsync();

        return logs;
    }

    public async Task<IEnumerable<LogResponse>> GetLogsByTagsAsync(string[] tags)
    {
        var db = _cosmicDbContext.CosmicLogs;

        var logs = await db
            .Where(cl => cl.Tags.Any(ct => tags.Contains(ct.Name)))
            .Select(cl => new LogResponse()
            {
                Id = cl.Id,
                Title = cl.Title,
                Content = cl.Content,
                Category = cl.Category,
                CreatedAt = cl.CreatedAt,
                CosmicEventId = cl.CosmicEventId,
                SourceUrl = cl.SourceUrl,
                Tags = cl.Tags
                    .Select(ct => new TagResponse()
                    {
                        Id = ct.Id,
                        Name = ct.Name
                    })
                    .ToList()
            })
            .ToListAsync();

        return logs;
    }

    public async Task<LogResponse?> CreateLogAsync(CreateLog newLog)
    {
        var db = _cosmicDbContext.CosmicLogs;
        var dbTags = _cosmicDbContext.CosmicTags;

        if (await db.AnyAsync(cl => cl.Title == newLog.Title))
        {
            return null;
        }

        var existingTags = await dbTags
            .Where(ct => newLog.Tags.Contains(ct.Name))
            .ToListAsync();

        var existingTagNames = existingTags
            .Select(ct => ct.Name)
            .ToList();

        var missingTags = newLog.Tags
            .Except(existingTagNames)
            .Select(s => new CosmicTag()
            {
                Name = s
            })
            .ToList();

        var cosmicLog = new CosmicLog()
        {
            Title = newLog.Title,
            Content = newLog.Content,
            Category = newLog.Category,
            CreatedAt = DateTime.Now,
            CosmicEventId = newLog.CosmicEventId,
            SourceUrl = newLog.SourceUrl,
            Tags = [.. existingTags, .. missingTags]
        };

        db.Add(cosmicLog);
        await _cosmicDbContext.SaveChangesAsync();

        var log = new LogResponse()
        {
            Id = cosmicLog.Id,
            Title = cosmicLog.Title,
            Content = cosmicLog.Content,
            Category = cosmicLog.Category,
            CreatedAt = cosmicLog.CreatedAt,
            CosmicEventId = cosmicLog.CosmicEventId,
            SourceUrl = cosmicLog.SourceUrl,
            Tags = [.. cosmicLog.Tags
                .Select(ct => new TagResponse()
                {
                    Id = ct.Id,
                    Name = ct.Name
                })]
        };

        return log;
    }
}
