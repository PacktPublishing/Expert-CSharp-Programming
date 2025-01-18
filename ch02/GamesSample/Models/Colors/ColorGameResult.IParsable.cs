using System.Diagnostics.CodeAnalysis;

namespace GamesSample.Models;

#if USERECORDS
public readonly partial record struct ColorGameResult : IParsable<ColorGameResult>
{
    public static ColorGameResult Parse(string s, IFormatProvider? provider = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(s);
        string[] parts = s.Split(Separator);
        if (parts.Length != 2)
        {
            throw new FormatException($"Input string was not in a correct format. Expected format: 'Correct:Incorrect', e.g. '1:1'. Actual: '{s}'");
        }

        if (!int.TryParse(parts[0], out int correctPosition))
        {
            throw new FormatException($"Invalid CorrectPosition value");
        }
        if (!int.TryParse(parts[1], out int inCorrectPosition))
        {
            throw new FormatException($"Invalid InCorrectPosition value");
        }
        return new ColorGameResult(correctPosition, inCorrectPosition);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ColorGameResult result)
    {
        if (s is not null)
        {
            try
            {
                result = Parse(s, provider);
                return true;
            }
            catch (FormatException)
            {
                result = default;
                return false;
            }
        }
        result = default;
        return false;
    }
}
#endif