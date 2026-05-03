namespace BackgroundServiceGC;

// ============================================================
// DataProcessingWorker — High-throughput message processing
//
// Demonstrates three GC anti-patterns and their fixes for
// background services that continuously process messages:
//
//  ❌ Anti-pattern 1: Allocating a new byte[] per message
//     → Short-lived, but large arrays often reach Gen-2 / LOH
//
//  ✅ Fix 1: Use a shared ArrayPool — rented buffers are
//     reused and never promoted to Gen-2
//
//  ❌ Anti-pattern 2: String concatenation in a loop
//     → Produces many intermediate Gen-0 strings that
//       increase GC pressure
//
//  ✅ Fix 2: Use Span<char> / stackalloc for small payloads
//     or StringBuilder for larger ones
//
//  ❌ Anti-pattern 3: Long-lived event subscriptions holding
//     references → objects cannot be collected (memory leak)
//
//  ✅ Fix 3: Implement IDisposable and unsubscribe in Dispose
// ============================================================
public sealed class DataProcessingWorker(
    ILogger<DataProcessingWorker> logger,
    MessageBufferPool bufferPool)
    : BackgroundService
{
    // Simulate 8 KB messages arriving every 5 ms
    private const int MessageSizeBytes = 8 * 1024;
    private static readonly TimeSpan MessageInterval = TimeSpan.FromMilliseconds(5);
    private static readonly TimeSpan ComparisonWindow = TimeSpan.FromSeconds(30);

    // Track allocation totals for comparison
    private long _allocatingBytesProcessed;
    private long _poolingBytesProcessed;
    private long _allocatingMessagesProcessed;
    private long _poolingMessagesProcessed;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("DataProcessingWorker started");

        ScenarioMetrics allocating = await RunScenarioAsync(
            RunWithNewArrayAsync,
            () => _allocatingBytesProcessed,
            () => _allocatingMessagesProcessed,
            stoppingToken).ConfigureAwait(false);

        // Reset GC counters between experiments to isolate the pooling run.
        ForceFullGc();

        ScenarioMetrics pooling = await RunScenarioAsync(
            RunWithPoolingAsync,
            () => _poolingBytesProcessed,
            () => _poolingMessagesProcessed,
            stoppingToken).ConfigureAwait(false);

        LogComparison(allocating, pooling);
        logger.LogInformation("DataProcessingWorker finished — waiting for host shutdown");

        // Keep the service alive so the host can shut it down cleanly
        await Task.Delay(Timeout.Infinite, stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
    }

    // ❌ Anti-pattern: new byte[] per message — each allocation
    // is a fresh Gen-0 object that can be promoted if it survives
    // one collection while still in use.
    private async Task RunWithNewArrayAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            byte[] buffer = new byte[MessageSizeBytes];   // heap allocation every tick
            ProcessMessage(buffer);
            Interlocked.Add(ref _allocatingBytesProcessed, MessageSizeBytes);
            Interlocked.Increment(ref _allocatingMessagesProcessed);

            try
            {
                await Task.Delay(MessageInterval, token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                break;
            }
        }
    }

    // ✅ Fix: rent a buffer from the pool, use it, return it —
    // the runtime reuses the same physical memory, so no new
    // Gen-0 objects are created after the pool warms up.
    private async Task RunWithPoolingAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            byte[] buffer = bufferPool.Rent(MessageSizeBytes);
            try
            {
                ProcessMessage(buffer);
                Interlocked.Add(ref _poolingBytesProcessed, MessageSizeBytes);
                Interlocked.Increment(ref _poolingMessagesProcessed);
            }
            finally
            {
                bufferPool.Return(buffer, clearArray: true);
            }

            try
            {
                await Task.Delay(MessageInterval, token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                break;
            }
        }
    }

    // Simulate non-trivial processing — XOR-fold the buffer
    private static void ProcessMessage(byte[] buffer)
    {
        byte accumulator = 0;
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(i & 0xFF);
            accumulator ^= buffer[i];
        }
        _ = accumulator; // prevent elimination
    }

    private async Task<ScenarioMetrics> RunScenarioAsync(
        Func<CancellationToken, Task> runScenario,
        Func<long> readProcessedBytes,
        Func<long> readProcessedMessages,
        CancellationToken stoppingToken)
    {
        long baselineBytes = readProcessedBytes();
        long baselineMessages = readProcessedMessages();

        long baselineAllocatedBytes = GC.GetTotalAllocatedBytes(precise: false);
        long baselineGenZeroCollections = GC.CollectionCount(0);
        long baselineGenOneCollections = GC.CollectionCount(1);

        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        cts.CancelAfter(ComparisonWindow);

        try
        {
            await runScenario(cts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            // Expected when the comparison window expires.
        }

        long processedBytes = readProcessedBytes() - baselineBytes;
        long processedMessages = readProcessedMessages() - baselineMessages;

        long allocatedBytes = GC.GetTotalAllocatedBytes(precise: false) - baselineAllocatedBytes;
        long genZeroCollections = GC.CollectionCount(0) - baselineGenZeroCollections;
        long genOneCollections = GC.CollectionCount(1) - baselineGenOneCollections;

        return new ScenarioMetrics(
            processedBytes / (1024.0 * 1024),
            processedMessages,
            allocatedBytes / (1024.0 * 1024),
            genZeroCollections,
            genOneCollections);
    }

    private void LogComparison(ScenarioMetrics allocating, ScenarioMetrics pooling)
    {
        logger.LogInformation(
            "Comparison after {Seconds}s each — \nAllocating: {AllocMB:F1} MB ({AllocMessages} msgs), " +
            "TotalAllocated: {AllocTotalMB:F1} MB, Gen0 Δ: {AllocG0}, Gen1 Δ: {AllocG1} | " +
            "\nPooling: {PoolMB:F1} MB ({PoolMessages} msgs), TotalAllocated: {PoolTotalMB:F1} MB, " +
            "Gen0 Δ: {PoolG0}, Gen1 Δ: {PoolG1}",
            ComparisonWindow.TotalSeconds,
            allocating.ProcessedMegabytes,
            allocating.ProcessedMessages,
            allocating.TotalAllocatedMegabytes,
            allocating.GenZeroCollections,
            allocating.GenOneCollections,
            pooling.ProcessedMegabytes,
            pooling.ProcessedMessages,
            pooling.TotalAllocatedMegabytes,
            pooling.GenZeroCollections,
            pooling.GenOneCollections);

        logger.LogInformation(
            "Takeaway: pooling should allocate far less managed memory. GC collection deltas can still be zero in short runs if the heap has enough free space.");
    }

    private void ForceFullGc()
    {
        logger.LogInformation("Force full GC");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    private readonly record struct ScenarioMetrics(
        double ProcessedMegabytes,
        long ProcessedMessages,
        double TotalAllocatedMegabytes,
        long GenZeroCollections,
        long GenOneCollections);
}
