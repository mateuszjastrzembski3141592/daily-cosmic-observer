namespace CosmicObserverAPI.Extensions;

public static class StringSanitizationExtensions
{
    public static string ToSanitizedString(this string input)
    {
        string result = input.Trim().ToLowerInvariant();

        var output = string.Concat(result.Where(c => char.IsAsciiLetterOrDigit(c) || c == ' '));

        var subStrings = output.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        output = String.Join('-', subStrings);

        return output;
    }
}
