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

/// <summary>Span parser: slices the original string — zero extra strings.</summary>
static class SpanParser
{
    public static void ParseAndPrint(string line)
    {
        // ✅ AsSpan() wraps the string's memory — no copy, no heap allocation
        ReadOnlySpan<char> span = line.AsSpan();

        ReadOnlySpan<char> secret = NextField(ref span, ',');
        ReadOnlySpan<char> guess  = NextField(ref span, ',');
        int black = int.Parse(NextField(ref span, ','));
        int white = int.Parse(span);                       // remaining field

        Console.WriteLine(
            $"  Span   parse → secret={secret}, guess={guess}, " +
            $"black={black}, white={white}");
    }

    // Advances 'span' past the delimiter and returns the leading field.
    // Returns the entire remaining span (and leaves span empty) if the
    // delimiter is not found, so the parser handles the last field safely.
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
