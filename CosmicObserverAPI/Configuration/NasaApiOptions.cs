namespace CosmicObserverAPI.Configuration;

public class NasaApiOptions
{
    public const string SectionName = "NasaApi";

    public required string ApiKey { get; set; }
}
