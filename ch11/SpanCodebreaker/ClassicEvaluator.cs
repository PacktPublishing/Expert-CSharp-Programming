// ============================================================
// SpanCodebreaker — Span<T> vs. Classic Array/String
//
// The Codebreaker game (similar to Mastermind®) requires
// evaluating a player's guess against a secret code:
//
//   • Black (exact) matches — right colour, right position
//   • White (partial) matches — right colour, wrong position
//
// Algorithm source: adapted from
//   https://github.com/codebreakerapp/codebreaker.Backend
//   (AnalyzerExtensions / GuessBoardExtensions)
//
// This sample shows the SAME algorithm implemented:
//  1. ClassicEvaluator  — uses arrays, List<T>, string copies
//  2. SpanEvaluator     — uses Span<T>, stackalloc, ReadOnlySpan
//
// Learning value:
//  • Span<T> lets you slice and process existing memory without
//    additional heap allocations.
//  • stackalloc gives you a small, fixed-size buffer on the
//    stack — zero GC involvement, zero heap pressure.
//  • ReadOnlySpan<char> lets you parse substrings of a string
//    without creating a new string object.
// ============================================================

// ============================================================
// Classic evaluator — reference implementation
// ============================================================

/// <summary>
/// Evaluates a Codebreaker guess using conventional arrays and a list.
/// Easy to read but allocates several heap objects per call.
/// </summary>
static class ClassicEvaluator
{
    public static (int Black, int White) Evaluate(string secret, string guess)
    {
        // ❌ ToCharArray allocates a new char[] on every call
        char[] secretChars = secret.ToCharArray();
        char[] guessChars  = guess.ToCharArray();

        int black = 0;
        // ❌ List<char> allocates an internal array plus the list header
        List<char> unmatchedSecret = [];
        List<char> unmatchedGuess  = [];

        // Pass 1: count exact (black) matches
        for (int i = 0; i < secretChars.Length; i++)
        {
            if (secretChars[i] == guessChars[i])
                black++;
            else
            {
                unmatchedSecret.Add(secretChars[i]);
                unmatchedGuess.Add(guessChars[i]);
            }
        }

        // Pass 2: count partial (white) matches from unmatched pegs
        int white = 0;
        foreach (char c in unmatchedGuess)
        {
            int idx = unmatchedSecret.IndexOf(c);
            if (idx >= 0)
            {
                white++;
                unmatchedSecret.RemoveAt(idx); // ❌ O(n) remove + array shift
            }
        }

        return (black, white);
    }
}
