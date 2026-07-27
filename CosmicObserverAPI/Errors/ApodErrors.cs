using CosmicObserverAPI.Enums;
using CosmicObserverAPI.Shared;

namespace CosmicObserverAPI.Errors;

public static class ApodErrors
{
    //public static Error EmptyDateError { get; } = new("Date can't be empty", "EmptyDateError", ErrorType.Validation);
    public static Error DateRangeError { get; } = new("StartDate can't come after EndDate", "DateRangeError", ErrorType.Validation);
    public static Error FutureDateError { get; } = new("Date can't be a future date", "FutureDateError", ErrorType.Validation);
    public static Error PastDateError { get; } = new("Date can't predate 16-06-1995", "PastDateError", ErrorType.Validation);

    public static Error ExternalRequestRejectionError { get; } = new("Request rejected by APOD API", "ExternalRequestRejectionError", ErrorType.Failure);
    public static Error EmptyResponseError { get; } = new("The response is empty", "EmptyResponseError", ErrorType.Failure);


    //public static Error FutureError(DateOnly date)
    //{
    //    return new($"Date can't be a future date ({date})", "FutureDateError", ErrorType.Validation);
    //}
}
