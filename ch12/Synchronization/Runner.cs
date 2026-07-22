namespace Synchronization;

internal class Runner
{
    /// <summary>
    /// Demonstrates the use of the lock keyword to ensure mutual exclusion for shared mutable state.
    /// </summary>
    /// <returns></returns>
    public static async Task LockKeywordAsync()
    {
        Lock l = new();
        // lock — mutual exclusion for shared mutable state
        Console.WriteLine("lock — thread-safe counter (no race condition)");
        Console.WriteLine("----------------------------------------------");
        Counter counter = new();
        await Task.WhenAll(
            Enumerable.Range(0, 10)
                .Select(_ => Task.Run(() => 
                { 
                    for (int i = 0; i < 1_000; i++) 
                        counter.Increment(); 
                })));

        Console.WriteLine($"Counter value: {counter.Value} (expected 10000)");
    }

    public static async Task InterlockedAsync()
    {
        // Interlocked — lock-free atomic operations (faster than lock for scalars)
        Console.WriteLine();
        Console.WriteLine("Interlocked — lock-free atomic increment");
        Console.WriteLine("----------------------------------------");
        long atomicCounter = 0;
        await Task.WhenAll(Enumerable.Range(0, 10).Select(_ =>
            Task.Run(() => { for (int i = 0; i < 1_000; i++) Interlocked.Increment(ref atomicCounter); })));
        Console.WriteLine($"Atomic counter: {atomicCounter} (expected 10000)");
    }

    public static async Task SemaphoreSlimAsync()
    {
        // SemaphoreSlim — throttle concurrent access (e.g., limit outbound HTTP calls)
        Console.WriteLine();
        Console.WriteLine("SemaphoreSlim — throttle to 3 concurrent requests");
        Console.WriteLine("----------------------------------------");
        using SemaphoreSlim throttle = new(initialCount: 3, maxCount: 3);
        int active = 0;
        int peak = 0;
        Lock peakLock = new();

        await Task.WhenAll(Enumerable.Range(1, 9).Select(async i =>
        {
            await throttle.WaitAsync();
            try
            {
                int current = Interlocked.Increment(ref active);
                lock (peakLock) peak = Math.Max(peak, current);
                await Task.Delay(Random.Shared.Next(20, 40));
                Interlocked.Decrement(ref active);
            }
            finally
            {
                throttle.Release();
            }
        }));
        Console.WriteLine($"Peak concurrency: {peak} (max allowed: 3)");
    }

    public static async Task ReaderWriterLockSlimAsync()
    {
        // ReaderWriterLockSlim — many concurrent readers, exclusive writer
        Console.WriteLine();
        Console.WriteLine("ReaderWriterLockSlim — concurrent reads, exclusive write");
        Console.WriteLine("--------------------------------------------------------");

        using SharedCache cache = new();
        Task[] readers = [.. Enumerable.Range(0, 5).Select(_ => Task.Run(() => cache.Read()))];
        Task writer = Task.Run(async () =>
        {
            await Task.Delay(20); // let readers start first
            cache.Write("updated-value");
        });

        await Task.WhenAll([.. readers, writer]);
        Console.WriteLine($"Cache value after write: '{cache.Read()}'");
    }

    public static async Task AvoidDeadlockAsync()
    {
        // Deadlock avoidance — always acquire locks in the same order
        Console.WriteLine();
        Console.WriteLine("Deadlock avoidance — always acquire locks in the same order");
        Console.WriteLine("-----------------------------------------------------------");
        Lock lockA = new();
        Lock lockB = new();

        // Safe: both tasks acquire lockA then lockB (consistent ordering)
        Task t1 = Task.Run(() =>
        {
            lock (lockA) 
            { 
                Thread.Sleep(10); 
                lock (lockB) 
                { 
                    Console.WriteLine("Task 1 holds A then B"); 
                } 
            }
        });
        Task t2 = Task.Run(() =>
        {
            lock (lockA) 
            { 
                Thread.Sleep(10); 
                lock (lockB) 
                { 
                    Console.WriteLine("Task 2 holds A then B"); 
                } 
            }
        });

        await Task.WhenAll(t1, t2);

        Console.WriteLine();
    }
}
