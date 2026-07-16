using CosmicObserverAPI.Enums;
using CosmicObserverAPI.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CosmicObserverAPI.Extensions;

public static class ControllerExtensions
{
    public static ActionResult ToErrorCode(this ControllerBase controllerBase, Error error) => error.Type switch
    {
        ErrorType.NotFound => controllerBase.NotFound(error),
        ErrorType.Validation => controllerBase.BadRequest(error),
        ErrorType.Conflict => controllerBase.Conflict(error),
        ErrorType.Failure => controllerBase.BadRequest(error),
        _ => controllerBase.StatusCode(500, error)
    };
}
