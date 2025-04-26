using System.Diagnostics.CodeAnalysis;

namespace CollectionTypes;

public readonly partial record struct RacerId : IParsable<RacerId>
{
    public static RacerId Parse(string s, IFormatProvider? provider = null)
    {
        if (TryParse(s, provider, out RacerId result))
        {
            return result;
        }

        throw new FormatException($"Invalid format: {s}");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out RacerId result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = default;
            return false;
        }

        string[] parts = s.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int id))
        {
            result = default;
            return false;
        }

        result = new RacerId(parts[0], id);
        return true;
    }
}
