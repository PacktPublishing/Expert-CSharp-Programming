using System.Collections.Concurrent;
using System.Threading.Channels;

namespace BestPractices;

internal class Runner
{
    public static async Task ParallelForEachAsync()
    {
        // Parallel.ForEachAsync — throttled async parallelism
        Console.WriteLine("Parallel.ForEachAsync — throttled parallel I/O");
        Console.WriteLine("----------------------------------------------");
        string[] urls = ["svc/a", "svc/b", "svc/c", "svc/d", "svc/e", "svc/f"];
        ConcurrentBag<string> collected = [];

        await Parallel.ForEachAsync(
            urls,
            new ParallelOptions { MaxDegreeOfParallelism = 3 },
            async (url, ct) =>
            {
                string data = await SimulateFetchAsync(url, delayMs: 40, ct);
                collected.Add(data);
            });

        Console.WriteLine($"Collected {collected.Count} responses");
    }

    public static async Task PLINQSampleAsync()
    {
        // PLINQ — data-parallel query on CPU-bound collections
        Console.WriteLine();
        Console.WriteLine("PLINQ — parallel image processing simulation");
        Console.WriteLine("--------------------------------------------");
        int[] imageIds = [.. Enumerable.Range(1, 200)];
        int[] processed = [.. imageIds
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount)
            .Select(id => ProcessImage(id))
            .OrderBy(x => x)];

        Console.WriteLine($"Processed {processed.Length} images; last id: {processed[^1]}");
    }

    public static async Task ChannelsAsync()
    {
        // System.Threading.Channels — high-throughput producer/consumer
        Console.WriteLine();
        Console.WriteLine("Channel<T> — producer/consumer pipeline");
        Console.WriteLine("---------------------------------------");
        Channel<WorkItem> channel = Channel.CreateBounded<WorkItem>(
            new BoundedChannelOptions(capacity: 10)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = false,
                SingleWriter = true
            });

        using CancellationTokenSource pipelineCts = new();

        Task producer = ProduceAsync(channel.Writer, count: 30, pipelineCts.Token);
        Task[] consumers =
        [
            ConsumeAsync(channel.Reader, consumerId: 1, pipelineCts.Token),
            ConsumeAsync(channel.Reader, consumerId: 2, pipelineCts.Token),
        ];

        await producer;
//         channel.Writer.Complete();
        await Task.WhenAll(consumers);
    }

    public static async Task LinkedCancellationTokenSourceAsync()
    { 
        // Linked CancellationTokenSource — compose multiple cancellation signals
        Console.WriteLine();
        Console.WriteLine("Linked CancellationTokenSource — timeout + manual cancel");
        Console.WriteLine("--------------------------------------------------------");

        using CancellationTokenSource userCts = new();

        using CancellationTokenSource timeoutCts = new(TimeSpan.FromMilliseconds(200));

        using CancellationTokenSource linked = CancellationTokenSource.CreateLinkedTokenSource(
            userCts.Token, timeoutCts.Token);

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(10), linked.Token);
        }
        catch (OperationCanceledException)
        {
            string reason = userCts.IsCancellationRequested ? "user cancel" : "timeout";
            Console.WriteLine($"Operation cancelled due to: {reason}");
        }

        Console.WriteLine();
    }

    private static async Task<string> SimulateFetchAsync(string url, int delayMs = 60, CancellationToken ct = default)
    {
        await Task.Delay(delayMs, ct);
        return $"200 OK from {url}";
    }

    private static int ProcessImage(int imageId)
    {
        // Simulate CPU-bound work (e.g., resize, filter)
        double acc = 0;

        for (int i = 0; i < 10_000; i++) 
            acc += Math.Sin(i * imageId);
        GC.KeepAlive(acc); // prevent the JIT from eliminating the loop as dead code
        return imageId;
    }

    static async Task ProduceAsync(ChannelWriter<WorkItem> writer, int count, CancellationToken ct)
    {
        for (int i = 1; i <= count; i++)
        {
            await writer.WriteAsync(new WorkItem(i, $"job-{i}"), ct);
            Console.WriteLine($"Producer: job-{i}");
            await Task.Delay(10, ct);
        }
        writer.Complete();
    }

    static async Task ConsumeAsync(ChannelReader<WorkItem> reader, int consumerId, CancellationToken ct)
    {
        await foreach (WorkItem item in reader.ReadAllAsync(ct))
        {
            await Task.Delay(25, ct); // simulate processing
            Console.WriteLine($"Consumer {consumerId}: processed {item.Payload}");
        }
    }

}
