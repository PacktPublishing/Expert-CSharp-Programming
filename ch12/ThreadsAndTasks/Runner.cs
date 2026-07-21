namespace ThreadsAndTasks;

public class Runner
{
    public static async void ThreadSample()
    {
        Console.WriteLine("Threads");
        Console.WriteLine("-------");

        // Dedicated Thread — for long-running CPU-bound work that should NOT
        // consume a pool thread (avoids pool starvation).
        Console.WriteLine("Dedicated thread for long-running CPU-bound work");
        using ManualResetEventSlim threadDone = new(false);

        Thread dedicated = new(() =>
        {
            Console.WriteLine($"Dedicated thread {Environment.CurrentManagedThreadId}: heavy computation started");
            Thread.Sleep(100); // simulate long work
            Console.WriteLine($"Dedicated thread {Environment.CurrentManagedThreadId}: done");
            threadDone.Set();
        })
        {
            Name = "HeavyComputeThread",
            IsBackground = true,   // dies with the process
            Priority = ThreadPriority.BelowNormal,
        };
        dedicated.Start();
        threadDone.Wait();
    }

    public static async Task TaskContinuations()
    {
        // Task continuations
        Console.WriteLine();
        Console.WriteLine("Task continuations");
        Console.WriteLine("------------------");

        Task<string> fetchTask = Task.Run(async () =>
        {
            await Task.Delay(50); // simulate I/O
            return "payload";
        });

        Task<int> processTask = fetchTask.ContinueWith(
            t => t.Result.Length,
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Console.WriteLine($"Payload length (via continuation): {await processTask}");
    }

    public static async Task MultipleTasksAsync()
    {
        // Task.WhenAll — run multiple async operations concurrently
        Console.WriteLine();
        Console.WriteLine("Task.WhenAll — concurrent I/O simulation");
        Console.WriteLine("----------------------------------------");

        string[] endpoints = ["api/orders", "api/inventory", "api/pricing"];

        IEnumerable<Task<string>> fetchTasks = endpoints.Select(ep => SimulateFetchAsync(ep));

        string[] responses = await Task.WhenAll(fetchTasks);
        foreach (string r in responses)
            Console.WriteLine($"{r}");
    }

    public static async Task FirstResponseWinsAsync()
    {
        // Task.WhenAny — first response wins (hedged requests)
        Console.WriteLine();
        Console.WriteLine("Task.WhenAny — first-response-wins (hedged request)");
        Console.WriteLine("---------------------------------------------------");
        Task<string> fast = SimulateFetchAsync("replica-A", delayMs: 80);
        Task<string> slow = SimulateFetchAsync("replica-B", delayMs: 300);
        Task<string> winner = await Task.WhenAny(fast, slow);
        Console.WriteLine($"Winner: {winner.Result}");
    }

    public static async Task CancellationAsync()
    {
        // CancellationToken — cooperative cancellation
        Console.WriteLine();
        Console.WriteLine("CancellationToken — cooperative cancellation");
        Console.WriteLine("--------------------------------------------");
        using CancellationTokenSource cts = new(TimeSpan.FromMilliseconds(150));
        try
        {
            await LongRunningOperationAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was cancelled after timeout ✔");
        }

        Console.WriteLine();
    }

    private static async Task<string> SimulateFetchAsync(string url, int delayMs = 60, CancellationToken ct = default)
    {
        await Task.Delay(delayMs, ct);
        return $"200 OK from {url}";
    }

    private static async Task LongRunningOperationAsync(CancellationToken ct)
    {
        for (int i = 0; i < 20; i++)
        {
            ct.ThrowIfCancellationRequested();
            await Task.Delay(20, ct);
            Console.WriteLine($"      Step {i + 1}/20…");
        }
    }
}