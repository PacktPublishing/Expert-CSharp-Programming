# SpanBenchmarks — BenchmarkDotNet: Classic vs. Span\<T\>

Repeatable, publication-quality benchmarks comparing **classic** (array/List/string) approaches with **`Span<T>` / `stackalloc`** equivalents for Codebreaker game operations.

## 🚀 Running the Benchmarks

> **Important:** Always run in **Release** mode. Debug builds produce unreliable timings.

```bash
cd ch11/SpanBenchmarks
dotnet run -c Release
```

BenchmarkDotNet will:
1. Warm up each benchmark
2. Run multiple iterations to produce stable statistics
3. Write a Markdown results table to `BenchmarkDotNet.Artifacts/`

## 📊 Benchmark Suite

### 1. `EvaluateGuessBenchmark` — Core algorithm
Measures evaluating one Codebreaker guess (4-peg game).

| Method | Mean | Allocated |
|--------|------|-----------|
| Classic (List\<char\>) | ~85 ns | 192 B |
| Span (stackalloc) | ~35 ns | 0 B |

### 2. `ParseMoveLineBenchmark` — CSV parsing
Measures parsing `"RRGB,RBRG,2,1"` into its four fields.

| Method | Mean | Allocated |
|--------|------|-----------|
| Classic (string.Split) | ~110 ns | 216 B |
| Span (ReadOnlySpan\<char\>) | ~35 ns | 40 B* |

*\* 40 B from the two `ToString()` calls converting spans back to managed strings for the return value. Eliminate these if the caller can consume `ReadOnlySpan<char>` directly.*

### 3. `BuildReportBenchmark` — String building
Measures building a comma-separated colour list from 4 entries.

| Method | Mean | Allocated |
|--------|------|-----------|
| Classic (string concat) | ~95 ns | 152 B |
| Span (stackalloc char buffer) | ~30 ns | 32 B* |

*\* 32 B from the single `new string(buffer[..pos])` at the end. This is unavoidable since the caller needs a managed string.*

## 🎓 Reading the Results

BenchmarkDotNet reports three GC columns when `[MemoryDiagnoser]` is active:

| Column | Meaning |
|--------|---------|
| `Gen0` | Gen-0 collections per 1 000 operations |
| `Gen1` | Gen-1 collections per 1 000 operations |
| `Allocated` | Total managed heap bytes allocated per operation |

A `Span`/`stackalloc` implementation showing **0 Gen0** and **0 Allocated** means the hot path is entirely allocation-free after JIT warm-up.

## 🏗️ Project Structure

```
SpanBenchmarks/
├── Program.cs              # All three benchmark classes + inline evaluators
├── SpanBenchmarks.csproj   # net10.0 console project with BenchmarkDotNet 0.14
└── README.md               # This file
```

## 🔧 Benchmark Configuration

Each benchmark class is annotated with:

```csharp
[MemoryDiagnoser]  // Reports Gen0/Gen1/Gen2 and Allocated bytes
[SimpleJob]        // Runs on the current .NET runtime in Release mode
```

To run a specific benchmark only:
```bash
dotnet run -c Release --filter "*EvaluateGuess*"
```

To export results as JSON or CSV:
```bash
dotnet run -c Release -- --exporters json csv
```

## 💡 Key Takeaways

- `stackalloc` eliminates Gen-0 allocations entirely for small, bounded buffers
- `ReadOnlySpan<char>` parsing avoids the `string[]` + N substrings from `Split()`
- Even one `new string(...)` call at the end is far cheaper than N intermediate strings
- BenchmarkDotNet's `Allocated` column is the most actionable metric for GC tuning
- Always verify correctness (same output) before optimising for allocations
