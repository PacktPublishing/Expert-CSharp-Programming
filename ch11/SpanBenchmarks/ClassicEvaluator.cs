// ============================================================
// SpanBenchmarks — BenchmarkDotNet: Classic vs. Span
//
// Run in Release mode for reliable results:
//   dotnet run -c Release
//
// The benchmarks measure three real-world Codebreaker tasks:
//
//  1. EvaluateGuess   — classic List<char> vs. stackalloc
//  2. ParseMoveLine   — string.Split() vs. Span slice
//  3. BuildReport     — string concatenation vs. Span/stackalloc
//
// Metrics reported:
//  • Mean elapsed time
//  • Allocated bytes (requires MemoryDiagnoser)
//  • Gen0/Gen1/Gen2 GC collections per 1 000 operations
// ============================================================

// ── Evaluator implementations (copied from SpanCodebreaker) ──────────────────

public static class ClassicEvaluator
{
    public static (int Black, int White) Evaluate(string secret, string guess)
    {
        char[] secretChars = secret.ToCharArray();
        char[] guessChars  = guess.ToCharArray();

        int black = 0;
        List<char> unmatchedSecret = [];
        List<char> unmatchedGuess  = [];

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

        int white = 0;
        foreach (char c in unmatchedGuess)
        {
            int idx = unmatchedSecret.IndexOf(c);
            if (idx >= 0)
            {
                white++;
                unmatchedSecret.RemoveAt(idx);
            }
        }

        return (black, white);
    }
}
