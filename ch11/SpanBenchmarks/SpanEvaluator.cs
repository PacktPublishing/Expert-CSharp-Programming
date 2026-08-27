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

public static class SpanEvaluator
{
    public static (int Black, int White) Evaluate(ReadOnlySpan<char> secret, ReadOnlySpan<char> guess)
    {
        Span<char> unmatchedSecret = stackalloc char[secret.Length];
        Span<char> unmatchedGuess  = stackalloc char[guess.Length];

        int black = 0;
        int usLen = 0;
        int ugLen = 0;

        for (int i = 0; i < secret.Length; i++)
        {
            if (secret[i] == guess[i])
                black++;
            else
            {
                unmatchedSecret[usLen++] = secret[i];
                unmatchedGuess[ugLen++]  = guess[i];
            }
        }

        int white = 0;
        Span<char> remaining = unmatchedSecret[..usLen];

        for (int i = 0; i < ugLen; i++)
        {
            int idx = remaining.IndexOf(unmatchedGuess[i]);
            if (idx >= 0)
            {
                white++;
                remaining[(idx + 1)..].CopyTo(remaining[idx..]);
                remaining = remaining[..(remaining.Length - 1)];
            }
        }

        return (black, white);
    }
}
