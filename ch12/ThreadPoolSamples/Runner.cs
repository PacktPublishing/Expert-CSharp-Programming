using System.Diagnostics;

namespace ThreadPoolSamples;

internal class Runner
{
    public static async Task UsingTheThreadPoolAsync()
    {
        Console.WriteLine("Using the Thread Pool");
        Console.WriteLine("---------------------");

        // The CLR thread pool recycles threads to avoid the cost of creating new
        // OS threads for every short-lived operation.

        // Queue a work item directly (fire-and-forget background work)
        Console.WriteLine("ThreadPool.QueueUserWorkItem — background logging");

        string[] items = ["order-41", "order-42", "order-43"];

        using SemaphoreSlim done = new(0, items.Length);

        foreach (var item in items)
        {
            ThreadPool.QueueUserWorkItem(ProcessItem, new WorkerState(item, done), preferLocal: false);
        }

        for (int i = 0; i < items.Length; i++)
        {
            await done.WaitAsync();
        }

        Console.WriteLine("done waiting for all items.");
    }

    internal record struct WorkerState(string Item, SemaphoreSlim Semaphore);

    private static void ProcessItem(WorkerState state)
    {
        (string item, SemaphoreSlim semaphore) = state;
        try
        {
            Thread.Sleep(300);
            Console.WriteLine($"Pool thread {Thread.CurrentThread.IsThreadPoolThread}, id: {Environment.CurrentManagedThreadId}: processing item '{item}'");
        }
        finally
        {
            semaphore.Release();
        }
    }

    public static async Task InspectingTheThreadPoolAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Thread pool sizing");
        Console.WriteLine("------------------");
        ThreadPool.GetMinThreads(out int minWorker, out int minIocp);
        ThreadPool.GetMaxThreads(out int maxWorker, out int maxIocp);
        ThreadPool.GetAvailableThreads(out int availableWorker, out int availableIocp);
       
        Console.WriteLine($"Min workers: {minWorker}, Max workers: {maxWorker}, Available workers: {availableWorker}");
        Console.WriteLine($"Min IOCP:    {minIocp},  Max IOCP:     {maxIocp}, Available IOCP:    {availableIocp}");

        Console.WriteLine($"Processor count: {Environment.ProcessorCount}");

        // Temporarily raise minimum to pre-warm threads for a burst workload
        int burstMinWorkers = Environment.ProcessorCount * 2;
        bool raised = ThreadPool.SetMinThreads(burstMinWorkers, minIocp);
        if (raised)
            Console.WriteLine($"Raised min workers to {burstMinWorkers} for burst workload");
        else
            Console.WriteLine($"Could not raise min workers to {burstMinWorkers} (OS or runtime limit)");

        bool restored = ThreadPool.SetMinThreads(minWorker, minIocp); // restore
        if (!restored)
            Console.WriteLine($"Warning: could not restore min workers to {minWorker}");
    }

    public static async Task TaskRunWithPoolAsync()
    {

        // Task.Run also routes through the pool
        Console.WriteLine();
        Console.WriteLine("Task.Run schedules onto the pool");
        Console.WriteLine("--------------------------------");
        int[] results = await Task.WhenAll(
            Enumerable.Range(1, 4).Select(i => Task.Run(() =>
            {
                Console.WriteLine($"Pool thread {Thread.CurrentThread.IsThreadPoolThread}, id: {Environment.CurrentManagedThreadId}: computing fib({i * 10})");
                return Math.Fibonacci(i * 10);
            })));

        Console.WriteLine($"Results: [{string.Join(", ", results)}]");
        Console.WriteLine();
    }
}
