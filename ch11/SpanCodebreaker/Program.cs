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

using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("🧩 Codebreaker: Span<T> vs. Classic Array Demo");
Console.WriteLine("=================================================");
Console.WriteLine();

// ── 1. Single-evaluation comparison ──────────────────────────
const string Secret = "RRGB";   // 4-peg game: Red Red Green Blue
const string Guess  = "RBRG";

(int classicBlack, int classicWhite) = ClassicEvaluator.Evaluate(Secret, Guess);
(int spanBlack,    int spanWhite)    = SpanEvaluator.Evaluate(Secret, Guess);

Console.WriteLine($"Secret: {Secret}  |  Guess: {Guess}");
Console.WriteLine($"Classic result:  Black={classicBlack}, White={classicWhite}");
Console.WriteLine($"Span    result:  Black={spanBlack},    White={spanWhite}");
Console.WriteLine(classicBlack == spanBlack && classicWhite == spanWhite
    ? "✅ Both evaluators agree."
    : "❌ Results differ!");
Console.WriteLine();

// ── 2. Allocation comparison ──────────────────────────────────
Console.WriteLine("Allocation comparison (100 000 evaluations each):");
Console.WriteLine("-------------------------------------------------");

const int Iterations = 100_000;

long allocBefore = GC.GetTotalAllocatedBytes(precise: true);
Stopwatch sw = Stopwatch.StartNew();
for (int i = 0; i < Iterations; i++)
    ClassicEvaluator.Evaluate(Secret, Guess);
sw.Stop();
long allocAfter = GC.GetTotalAllocatedBytes(precise: true);

Console.WriteLine($"  Classic: {sw.ElapsedMilliseconds,5} ms | allocated {(allocAfter - allocBefore),14:N0} bytes");

allocBefore = GC.GetTotalAllocatedBytes(precise: true);
sw.Restart();
for (int i = 0; i < Iterations; i++)
    SpanEvaluator.Evaluate(Secret, Guess);
sw.Stop();
allocAfter = GC.GetTotalAllocatedBytes(precise: true);

Console.WriteLine($"  Span:    {sw.ElapsedMilliseconds,5} ms | allocated {(allocAfter - allocBefore),14:N0} bytes");
Console.WriteLine();

// ── 3. String parsing demo ────────────────────────────────────
Console.WriteLine("Parsing a CSV move line with Span<char>:");
Console.WriteLine("-----------------------------------------");
const string MoveLine = "RRGB,RBRG,2,1";   // format: secret,guess,black,white

ClassicParser.ParseAndPrint(MoveLine);
SpanParser.ParseAndPrint(MoveLine);
Console.WriteLine();

Console.WriteLine("✅ Key Takeaways:");
Console.WriteLine("  • Span<T> evaluator allocates 0 bytes after warm-up (stackalloc)");
Console.WriteLine("  • ReadOnlySpan<char>.Slice() avoids creating substring strings");
Console.WriteLine("  • stackalloc is ideal for small, fixed-size scratch buffers");
Console.WriteLine("  • Span<T> cannot be stored in fields — use Memory<T> for async paths");
