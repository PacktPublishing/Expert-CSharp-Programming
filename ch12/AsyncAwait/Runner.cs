using System.Runtime.CompilerServices;

namespace AsyncAwait;

internal class Runner
{
    public static async Task AsyncAndAwaitAsync()
    {
        // Basic async/await pipeline
        Console.WriteLine("Async pipeline — fetch → parse → store");
        Console.WriteLine("--------------------------------------");
        string raw = await FetchDataAsync("https://example.api/data");
        ParsedRecord record = ParseRecord(raw);
        await StoreAsync(record);
        Console.WriteLine($"Stored: {record}");
    }

    public static async Task ValueTaskSampleAsync()
    {
        // ValueTask — avoids heap allocation when the result is often synchronous
        Console.WriteLine();
        Console.WriteLine("ValueTask — zero-allocation fast path");
        Console.WriteLine("-------------------------------------");
        AsyncCache<string> valueCache = new();
        // First call misses cache (async path)
        string v1 = await valueCache.GetOrFetchAsync("key", async () =>
        {
            await Task.Delay(20);
            return "fetched-value";
        });
        // Second call hits cache (sync path — no Task allocation)
        string v2 = await valueCache.GetOrFetchAsync("key", async () =>
        {
            await Task.Delay(20);
            return "should-not-be-called";
        });
        Console.WriteLine($"v1={v1}, v2={v2} (same value, second was sync)");
    }

    public static async Task ConfigureAwaitSampleAsync()
    {
        // ConfigureAwait(false) — avoid capturing the synchronization context
        //     in library code to prevent deadlocks in UI / ASP.NET Classic apps
        Console.WriteLine();
        Console.WriteLine("ConfigureAwait(false) — library pattern");
        Console.WriteLine("---------------------------------------");
        string result = await LibraryMethodAsync().ConfigureAwait(false);
        Console.WriteLine($"Library result: {result}");
    }

    public static async Task AsyncStreaming()
    { 
        // IAsyncEnumerable — async streaming (pull-based)
        Console.WriteLine();
        Console.WriteLine("IAsyncEnumerable — streaming sensor readings");
        Console.WriteLine("--------------------------------------------");

        CancellationTokenSource cts = new(TimeSpan.FromMilliseconds(90));

        IAsyncEnumerable<SensorReading> stream = ReadSensorsAsync(5);

        try
        {
            await foreach (SensorReading reading in stream.WithCancellation(cts.Token))
            {
                Console.WriteLine($"{reading}");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("streaming cancelled before it ended");
        }

        Console.WriteLine();
    }

    private static async Task<string> FetchDataAsync(string url)
    {
        await Task.Delay(30); // simulate HTTP call
        return $"{{\"source\":\"{url}\",\"value\":42}}";
    }

    private static ParsedRecord ParseRecord(string raw) =>
        new(Source: raw, Length: raw.Length);

    private static async Task StoreAsync(ParsedRecord record)
    {
        await Task.Delay(600); // simulate DB write
        _ = record;
    }

    private static async Task<string> LibraryMethodAsync()
    {
        // Library code always uses ConfigureAwait(false) to avoid capturing
        // the caller's SynchronizationContext and causing deadlocks in UI apps.
        await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: false);
        return "library-result";
    }

    private static async IAsyncEnumerable<SensorReading> ReadSensorsAsync(
        int count,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        for (int i = 1; i <= count; i++)
        {
            ct.ThrowIfCancellationRequested();
            await Task.Delay(20, ct); // simulate sensor polling
            yield return new SensorReading(
                Id: i,
                Temperature: 20.0 + Random.Shared.NextDouble() * 10,
                Timestamp: DateTimeOffset.UtcNow);
        }
    }
}
