namespace CosmicObserverAPI.Shared;

public class Error
{
    public string Message { get; }
    public string Code { get; }

    public Error(string message, string code)
    {
        Message = message;
        Code = code;
    }
}
