using CosmicObserverAPI.DTOs.CosmicEvent;
using CosmicObserverAPI.Models;
using System.Linq.Expressions;

namespace CosmicObserverAPI.Extensions;

public static class EventMappingExtensions
{
    public static Expression<Func<CosmicEvent, EventResponse>> ToEventResponseExpression =>
        ce => new EventResponse()
        {
            Id = ce.Id,
            Title = ce.Title,
            Description = ce.Description,
            Date = ce.Date,
            ImageUrl = ce.ImageUrl,
            SourceUrl = ce.SourceUrl
        };
}
