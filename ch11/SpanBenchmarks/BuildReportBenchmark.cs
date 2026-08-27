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
// ── Benchmark 3: Report building ──────────────────────────────────────────────

/// <summary>
/// Measures building a comma-separated colour list (e.g., for a log line).
/// Naive concatenation creates N intermediate strings;
/// Span / stackalloc builds in-place with a single final ToString().
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class BuildReportBenchmark
{
    private static readonly string[] Colours = ["Red", "Red", "Green", "Blue"];

    [Benchmark(Baseline = true, Description = "Classic (string concat)")]
    public string Classic()
    {
        string result = "";
        for (int i = 0; i < Colours.Length; i++)
        {
            if (i > 0) result += ",";
            result += Colours[i];             // ❌ new string per iteration
        }
        return result;
    }

    [Benchmark(Description = "Span (stackalloc char buffer)")]
    public string Span()
    {
        // 64 chars is enough for 4-6 colour names plus separators
        Span<char> buffer = stackalloc char[64];
        int pos = 0;

        for (int i = 0; i < Colours.Length; i++)
        {
            if (i > 0)
                buffer[pos++] = ',';

            ReadOnlySpan<char> colour = Colours[i].AsSpan();
            colour.CopyTo(buffer[pos..]);
            pos += colour.Length;
        }

        return new string(buffer[..pos]);     // one allocation for the result
    }
}
