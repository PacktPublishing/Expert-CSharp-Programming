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
// Span-based evaluator — zero-allocation hot path
// ============================================================

/// <summary>
/// Evaluates a Codebreaker guess using Span&lt;T&gt; and stackalloc.
/// After the JIT warms up this path allocates 0 bytes on the heap.
/// </summary>
static class SpanEvaluator
{
    public static (int Black, int White) Evaluate(ReadOnlySpan<char> secret, ReadOnlySpan<char> guess)
    {
        // ✅ stackalloc: memory lives on the call stack, not the heap.
        //    Safe here because the Codebreaker game rules cap code length at 8 pegs,
        //    so the buffer is bounded by a small known maximum (not unbounded input).
        //    For truly unbounded or large inputs, use ArrayPool<T> instead.
        Span<char> unmatchedSecret = stackalloc char[secret.Length];
        Span<char> unmatchedGuess  = stackalloc char[guess.Length];

        int black = 0;
        int usLen = 0; // logical length of unmatchedSecret
        int ugLen = 0; // logical length of unmatchedGuess

        // Pass 1: collect non-matching pegs
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

        // Pass 2: count partial matches — O(n²) but n ≤ 8 in practice
        int white = 0;
        Span<char> remaining = unmatchedSecret[..usLen]; // ✅ slice, no copy

        for (int i = 0; i < ugLen; i++)
        {
            int idx = remaining.IndexOf(unmatchedGuess[i]);
            if (idx >= 0)
            {
                white++;
                // Remove by shifting left — stays on the stack
                remaining[(idx + 1)..].CopyTo(remaining[idx..]);
                remaining = remaining[..(remaining.Length - 1)];
            }
        }

        return (black, white);
    }
}
