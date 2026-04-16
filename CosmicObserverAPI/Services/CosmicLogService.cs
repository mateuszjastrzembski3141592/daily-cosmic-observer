using CosmicObserverAPI.Data;
using CosmicObserverAPI.DTOs.CosmicLog;
using CosmicObserverAPI.DTOs.CosmicTag;
using CosmicObserverAPI.Enums;
using CosmicObserverAPI.Extensions;
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
            .Select(LogMappingExtensions.ToLogResponseExpression)
            .ToListAsync();

        return logs;
    }

    public async Task<LogResponse?> GetLogByIdAsync(int id)
    {
        var db = _cosmicDbContext.CosmicLogs;

        var log = await db
            .Where(cl => cl.Id == id)
            .Select(LogMappingExtensions.ToLogResponseExpression)
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
            .Select(LogMappingExtensions.ToLogResponseExpression)
            .ToListAsync();

        return logs;
    }

    public async Task<IEnumerable<LogResponse>> GetLogsByTagsAsync(string[] tags)
    {
        var db = _cosmicDbContext.CosmicLogs;

        var logs = await db
            .Where(cl => cl.Tags.Any(ct => tags.Contains(ct.Name)))
            .Select(LogMappingExtensions.ToLogResponseExpression)
            .ToListAsync();

        return logs;
    }

    public async Task<LogResponse?> CreateLogAsync(CreateLog newLog)
    {
        var db = _cosmicDbContext.CosmicLogs;

        if (await db.AnyAsync(cl => cl.Title == newLog.Title))
        {
            return null;
        }

        var cosmicLog = new CosmicLog()
        {
            Title = newLog.Title,
            Content = newLog.Content,
            Category = newLog.Category,
            CreatedAt = DateTime.Now,
            CosmicEventId = newLog.CosmicEventId,
            SourceUrl = newLog.SourceUrl,
            Tags = await GetOrCreateTagsAsync(newLog.Tags)
        };

        db.Add(cosmicLog);
        await _cosmicDbContext.SaveChangesAsync();

        var log = cosmicLog.ToLogResponse();

        return log;
    }

    public async Task<LogResponse?> UpdateLogAsync(CreateLog newLog, int id)
    {
        var db = _cosmicDbContext.CosmicLogs;

        var cosmicLog = await db.Include(cl => cl.Tags).FirstOrDefaultAsync(cl => cl.Id == id);

        if (cosmicLog is null)
        {
            return null;
        }

        if (await db.AnyAsync(cl => cl.Title == newLog.Title && cl.Id != id))
        {
            return null;
        }

        cosmicLog.Title = newLog.Title;
        cosmicLog.Content = newLog.Content;
        cosmicLog.Category = newLog.Category;
        cosmicLog.CosmicEventId = newLog.CosmicEventId;
        cosmicLog.SourceUrl = newLog.SourceUrl;
        cosmicLog.Tags = await GetOrCreateTagsAsync(newLog.Tags);

        await _cosmicDbContext.SaveChangesAsync();

        var log = cosmicLog.ToLogResponse();

        return log;
    }

    public async Task<bool> DeleteLogAsync(int id)
    {
        var db = _cosmicDbContext.CosmicLogs;

        var log = await db.FindAsync(id);

        if (log is null)
        {
            return false;
        }

        db.Remove(log);
        await _cosmicDbContext.SaveChangesAsync();

        return true;
    }

    private async Task<List<CosmicTag>> GetOrCreateTagsAsync(IEnumerable<string> newTags)
    {
        var dbTags = _cosmicDbContext.CosmicTags;

        var existingTags = await dbTags
            .Where(ct => newTags.Contains(ct.Name))
            .ToListAsync();

        var existingTagNames = existingTags
            .Select(ct => ct.Name)
            .ToList();

        var missingTags = newTags
            .Except(existingTagNames)
            .Select(s => new CosmicTag()
            {
                Name = s
            })
            .ToList();

        return [.. existingTags, .. missingTags];
    }
}