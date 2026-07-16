using CosmicObserverAPI.Enums;

namespace CosmicObserverAPI.Shared;

public class Error
{
    public string Message { get; }
    public string Code { get; }
    public ErrorType Type { get; }

    public Error(string message, string code, ErrorType type)
    {
        Message = message;
        Code = code;
        Type = type;
    }
}
