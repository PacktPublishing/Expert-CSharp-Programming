# SpanCodebreaker — Span\<T\> vs. Classic Arrays

Demonstrates how **`Span<T>` and `ReadOnlySpan<char>`** eliminate heap allocations in a realistic algorithm adapted from the [Codebreaker backend](https://github.com/codebreakerapp/codebreaker.Backend).

## 🎯 The Algorithm

The Codebreaker (Mastermind®) evaluator compares a hidden *secret* code to a player's *guess*:

- **Black pegs** — correct colour **and** correct position
- **White pegs** — correct colour but **wrong** position

Example:
```
Secret: R R G B
Guess:  R B R G
Result: 1 black, 3 white
```

## 🚀 What's Inside

| Class | Approach | Allocations per call |
|-------|----------|---------------------|
| `ClassicEvaluator` | `ToCharArray()`, `List<char>`, `RemoveAt()` | ~3 heap objects |
| `SpanEvaluator` | `stackalloc char[n]`, `Span<char>.IndexOf` | **0 bytes** |
| `ClassicParser` | `string.Split(',')` | 1 array + 4 strings |
| `SpanParser` | `ReadOnlySpan<char>` slicing | 0 extra strings |

## 🎓 Skills Demonstrated

### Skill 1 — stackalloc for small fixed-size scratch buffers
```csharp
// ✅ Stack-allocated scratch space — no heap, no GC
Span<char> unmatchedSecret = stackalloc char[secret.Length];
Span<char> unmatchedGuess  = stackalloc char[guess.Length];
```
**Rule of thumb**: use `stackalloc` for buffers ≤ ~1 KB whose size is bounded by a known small maximum (e.g., game rules limiting code length to 8 pegs). For larger, unbounded, or highly variable inputs, prefer `ArrayPool<T>` over `stackalloc` to avoid stack overflow risks.

### Skill 2 — ReadOnlySpan\<char\> as a zero-copy string view
```csharp
// ✅ AsSpan() wraps the string's memory — no allocation
ReadOnlySpan<char> span = moveLine.AsSpan();

// Slice fields without creating substrings
ReadOnlySpan<char> secret = span[..4];     // "RRGB" — still inside the original string
int black = int.Parse(span[10..11]);       // parse directly from the span
```

### Skill 3 — In-place array manipulation on the stack
```csharp
// Remove matched element by shifting — stays on the stack
remaining[(idx + 1)..].CopyTo(remaining[idx..]);
remaining = remaining[..(remaining.Length - 1)];
```

### Skill 4 — When to use Memory\<T\> instead of Span\<T\>
`Span<T>` is a **ref struct** — it cannot be stored in fields or used across `await` boundaries.  
Use `Memory<T>` when you need to:
- Store a slice as a class field
- Pass the slice to an `async` method
- Use it with `IMemoryOwner<T>` from `MemoryPool<T>`

```csharp
// ❌ Won't compile — Span cannot cross async boundaries
async Task ProcessAsync(Span<char> data) { await SomeAsync(); }

// ✅ Memory<T> can cross async boundaries
async Task ProcessAsync(Memory<char> data) { await SomeAsync(); }
```

## 🏗️ Project Structure

```
SpanCodebreaker/
├── Program.cs              # All evaluator and parser implementations
├── SpanCodebreaker.csproj  # net10.0 console project
└── README.md               # This file
```

## ▶️ Running the Sample

```bash
cd ch11/SpanCodebreaker
dotnet run
```

## 📊 Sample Output

```
🧩 Codebreaker: Span<T> vs. Classic Array Demo
=================================================

Secret: RRGB  |  Guess: RBRG
Classic result:  Black=1, White=3
Span    result:  Black=1,    White=3
✅ Both evaluators agree.

Allocation comparison (100 000 evaluations each):
-------------------------------------------------
  Classic:    26 ms | allocated     19,200,592 bytes
  Span:       19 ms | allocated              0 bytes

Parsing a CSV move line with Span<char>:
-----------------------------------------
  Classic parse → secret=RRGB, guess=RBRG, black=2, white=1
  Span   parse  → secret=RRGB, guess=RBRG, black=2, white=1

✅ Key Takeaways:
  • Span<T> evaluator allocates 0 bytes after warm-up (stackalloc)
  • ReadOnlySpan<char>.Slice() avoids creating substring strings
  • stackalloc is ideal for small, fixed-size scratch buffers
  • Span<T> cannot be stored in fields — use Memory<T> for async paths
```

## 💡 Key Takeaways

| Technique | Use case | Benefit |
|-----------|----------|---------|
| `stackalloc` | Small, bounded scratch buffers | Zero heap allocation |
| `Span<T>` slicing | Read/write existing memory | Zero copy |
| `ReadOnlySpan<char>` | String parsing | Zero substring strings |
| `Memory<T>` | Async / stored slices | Safe across await |
| `ArrayPool<T>` | Large variable-size buffers | Reuse without LOH |
