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
// ── Benchmark 1: Guess evaluation ─────────────────────────────────────────────

/// <summary>
/// Measures the cost of evaluating one Codebreaker guess.
/// Classic approach allocates 3+ heap objects per call;
/// Span approach uses only stack memory after JIT warm-up.
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class EvaluateGuessBenchmark
{
    private const string Secret = "RRGB";
    private const string Guess  = "RBRG";

    [Benchmark(Baseline = true, Description = "Classic (List<char>)")]
    public (int, int) Classic() => ClassicEvaluator.Evaluate(Secret, Guess);

    [Benchmark(Description = "Span (stackalloc)")]
    public (int, int) Span() => SpanEvaluator.Evaluate(Secret, Guess);
}
