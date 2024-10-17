
using System.Diagnostics.CodeAnalysis;

namespace GamesSample.Models;

public readonly partial record struct ShapeResult : IParsable<ShapeResult>
{
    public static ShapeResult Parse(string s, IFormatProvider? provider = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(s);
        string[] parts = s.Split(Separator);
        if (parts.Length != 3)
        {
            throw new FormatException($"Input string was not in a correct format. Expected format: 'Correct:Incorrect:PartialCorrect', e.g. '1:1:0'. Actual: '{s}'");
        }

        if (!int.TryParse(parts[0], out int correctPosition))
        {
            throw new FormatException($"Invalid CorrectPosition value");
        }
        if (!int.TryParse(parts[1], out int inCorrectPosition))
        {
            throw new FormatException($"Invalid InCorrectPosition value");
        }
        if (!int.TryParse(parts[2], out int partialCorrect))
        {
            throw new FormatException($"Invalid InCorrectPosition value");
        }
        return new ShapeResult(correctPosition, inCorrectPosition, partialCorrect);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ShapeResult result)
    {
        bool InvalidFormat(out ShapeResult result)
        {
            result = default;
            return false;
        }

        if (s is not null)
        {
            try
            {
                result = Parse(s, provider);
                return true;
            }
            catch (FormatException)
            {
                return InvalidFormat(out result);
            }
        }
        return InvalidFormat(out result);
    }
}

