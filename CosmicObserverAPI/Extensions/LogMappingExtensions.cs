using CosmicObserverAPI.DTOs.CosmicLog;
using CosmicObserverAPI.DTOs.CosmicTag;
using CosmicObserverAPI.Models;
using System.Linq.Expressions;

namespace CosmicObserverAPI.Extensions;

public static class LogMappingExtensions
{
    public static LogResponse ToLogResponse(this CosmicLog log)
    {
        return new LogResponse()
        {
            Id = log.Id,
            Title = log.Title,
            Content = log.Content,
            Category = log.Category,
            CreatedAt = log.CreatedAt,
            CosmicEventId = log.CosmicEventId,
            SourceUrl = log.SourceUrl,
            Tags = [.. log.Tags
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
