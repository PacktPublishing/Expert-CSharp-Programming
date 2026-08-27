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

using BenchmarkDotNet.Attributes;
// ── Benchmark 2: Move line parsing ────────────────────────────────────────────

/// <summary>
/// Measures parsing a CSV move line "RRGB,RBRG,2,1".
/// string.Split() creates a string[] + 4 substrings;
/// Span-based parsing performs zero extra heap allocations until fields are materialized as strings.
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class ParseMoveLineBenchmark
{
    private const string MoveLine = "RRGB,RBRG,2,1";

    [Benchmark(Baseline = true, Description = "Classic (string.Split)")]
    public (string, string, int, int) Classic()
    {
        string[] parts = MoveLine.Split(',');
        return (parts[0], parts[1], int.Parse(parts[2]), int.Parse(parts[3]));
    }

    [Benchmark(Description = "Span (ReadOnlySpan<char>)")]
    public (string, string, int, int) Span()
    {
        ReadOnlySpan<char> span = MoveLine.AsSpan();

        ReadOnlySpan<char> secret = NextField(ref span, ',');
        ReadOnlySpan<char> guess  = NextField(ref span, ',');
        int black = int.Parse(NextField(ref span, ','));
        int white = int.Parse(span);

        // ToString() only called here if a string result is needed downstream
        return (secret.ToString(), guess.ToString(), black, white);
    }

    private static ReadOnlySpan<char> NextField(ref ReadOnlySpan<char> span, char delimiter)
    {
        int idx = span.IndexOf(delimiter);
        if (idx < 0)
        {
            ReadOnlySpan<char> last = span;
            span = ReadOnlySpan<char>.Empty;
            return last;
        }
        ReadOnlySpan<char> field = span[..idx];
        span = span[(idx + 1)..];
        return field;
    }
}
