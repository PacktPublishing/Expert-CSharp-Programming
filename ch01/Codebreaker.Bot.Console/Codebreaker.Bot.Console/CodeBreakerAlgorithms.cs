using System.Runtime.CompilerServices;

namespace CodeBreaker.Bot;

public record struct KeyPegWithFlag(string Value, bool Used);

public static class CodeBreakerAlgorithms
{
    /// <summary>
    /// Reduces the possible values based on the black matches with the selection
    /// </summary>
    /// <param name="values">The list of possible moves</param>
    /// <param name="blackHits">The number of black hits with the selection</param>
    /// <param name="selection">The key pegs of the selected move</param>
    /// <returns>The remaining possible moves</returns>
    /// <exception cref="ArgumentException"></exception>
    public static List<string[]> ProcessBlackMatches(this IList<string[]> values, int blackHits, string[] guesses)
    {
        if (blackHits is < 1 or > 3)
        {
            throw new ArgumentException("invalid argument - hits need to be between 1 and 3");
        }

        List<string[]> matchingValues1 = values
            .Where(fields => fields.Where((color, ix) => color == guesses[ix]).Count() == blackHits)
            .ToList();

        List<string[]> matchingValues = [];
        foreach (var fields in values)
        {
            int count = 0;
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] == guesses[i])
                {
                    count++;
                }
            }
            if (count == blackHits)
            {
                matchingValues.Add(fields);
            }
        }

        return matchingValues;
    }

    /// <summary>
    /// Reduces the possible values based on the white matches with the selection
    /// </summary>
    /// <param name="values">The possible values</param>
    /// <param name="whiteBlackHits">The number of white hits with the selection</param>
    /// <param name="selection">The selected pegs</param>
    /// <returns>The remaining possible values</returns>
    public static List<string[]> ProcessWhiteMatches(this IList<string[]> values, int whiteBlackHits, string[] guesses)
    {
        if (whiteBlackHits is < 1 or > 4)
        {
            throw new ArgumentException("invalid argument - hits need to be between 1 and 4");
        }

        List<string[]> matchingValues = new(values.Count);  // max size needed of the previous collection
        foreach (var value in values)
        {
            KeyPegArray guesses1 = new();
            for (int i = 0; i < 4; i++)
            {
                guesses1[i] = new KeyPegWithFlag(guesses[i], false);
            }

            KeyPegArray row = new();
            for (int i = 0; i < 4; i++)
            {
                row[i] = new KeyPegWithFlag(value[i], false);
            }

            int matchCount = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!row[i].Used && !guesses1[j].Used && row[i].Value == guesses1[j].Value)
                    {
                        matchCount++;
                        row[i] = row[i] with { Used = true };
                        guesses1[j] = guesses1[j] with { Used = true };
                    }
                }
                if (matchCount == whiteBlackHits)
                {
                    matchingValues.Add(value);
                }
            }
        }
        return matchingValues;
    }

    /// <summary>
    /// Reduces the possible values if no selection was correct
    /// </summary>
    /// <param name="values">The possible values</param>
    /// <param name="guesses">The selected pegs</param>
    /// <returns>The remaining possible values</returns>
    public static List<string[]> ProcessNoMatches(this IList<string[]> values, string[] guesses)
    {
        List<string[]> matchingValues;
        matchingValues = values.Where(fields => !fields.Intersect(guesses).Any()).ToList();
        return matchingValues;
    }

}

[InlineArray(4)]
internal struct KeyPegArray
{
    private KeyPegWithFlag _keyPegWithFlag;
}