using CosmicObserverAPI.DTOs.CosmicLog;
using CosmicObserverAPI.DTOs.CosmicTag;
using CosmicObserverAPI.Models;
using System.Linq.Expressions;

namespace CosmicObserverAPI.Extensions;

public static class LogMappingExtensions
{
    public static LogResponse ToLogResponse(this CosmicLog cosmicLog)
    {
        return new LogResponse()
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
    }

    public static Expression<Func<CosmicLog, LogResponse>> ToLogResponseExpression =>
        cl => new LogResponse()
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
        };

    public static CosmicLog ToLogEntity(this CreateLog newLog, List<CosmicTag> tags)
    {
        return new CosmicLog()
        {
            Title = newLog.Title,
            Content = newLog.Content,
            Category = newLog.Category,
            CreatedAt = DateTime.Now,
            CosmicEventId = newLog.CosmicEventId,
            SourceUrl = newLog.SourceUrl,
            Tags = tags
        };
    }

    public static void UpdateLogEntity(this CosmicLog cosmicLog, CreateLog newLog, List<CosmicTag> tags)
    {
        cosmicLog.Title = newLog.Title;
        cosmicLog.Content = newLog.Content;
        cosmicLog.Category = newLog.Category;
        cosmicLog.CosmicEventId = newLog.CosmicEventId;
        cosmicLog.SourceUrl = newLog.SourceUrl;
        cosmicLog.Tags = tags;
    }
}
