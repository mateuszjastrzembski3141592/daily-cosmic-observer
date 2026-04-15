using CosmicObserverAPI.Data;
using CosmicObserverAPI.DTOs.CosmicLog;
using CosmicObserverAPI.DTOs.CosmicTag;
using CosmicObserverAPI.Enums;
using CosmicObserverAPI.Interfaces;
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
}
