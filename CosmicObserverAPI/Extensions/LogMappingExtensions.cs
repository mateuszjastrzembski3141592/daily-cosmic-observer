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
}
