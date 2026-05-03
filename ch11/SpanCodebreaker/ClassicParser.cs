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
// CSV line parsers
// ============================================================

/// <summary>Classic parser: splits the string and creates substrings.</summary>
static class ClassicParser
{
    public static void ParseAndPrint(string line)
    {
        // ❌ Split allocates a string[] and 4 individual strings
        string[] parts = line.Split(',');
        Console.WriteLine(
            $"  Classic parse → secret={parts[0]}, guess={parts[1]}, " +
            $"black={int.Parse(parts[2])}, white={int.Parse(parts[3])}");
    }
}
