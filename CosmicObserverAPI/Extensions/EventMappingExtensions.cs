using CosmicObserverAPI.DTOs.Apod;
using CosmicObserverAPI.DTOs.CosmicEvent;
using CosmicObserverAPI.Models;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public static CosmicEvent ToEventEntity(this NasaApodResponse apodResponse)
    {
        return new CosmicEvent()
        {
            Title = apodResponse.Title,
            Description = apodResponse.Description,
            Date = apodResponse.Date,
            ImageUrl = apodResponse.ImageUrl,
            SourceUrl = $"https://apod.nasa.gov/apod/ap{apodResponse.Date:yyMMdd}.html"
        };
    }
}
