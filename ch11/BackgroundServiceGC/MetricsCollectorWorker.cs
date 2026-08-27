using System.Runtime;

namespace BackgroundServiceGC;

// ============================================================
// MetricsCollectorWorker — Periodic GC monitoring
//
// Production background services must monitor their own memory
// health. This worker runs every 30 s and logs GC stats so
// operators can see heap growth, fragmentation and collection
// counts without attaching a profiler.
//
// Key GC skill: reading GCMemoryInfo and collection counts.
// ============================================================
public sealed class MetricsCollectorWorker(ILogger<MetricsCollectorWorker> logger)
    : BackgroundService
{
    private static readonly TimeSpan ReportInterval = TimeSpan.FromSeconds(3);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("MetricsCollectorWorker started — will report GC stats every {Interval}s",
            ReportInterval.TotalSeconds);

        // Use SustainedLowLatency during the whole worker lifetime so that
        // Gen-2 collections do not cause large pauses while the service is
        // also processing real-time messages on other threads.
        GCLatencyMode previous = GCSettings.LatencyMode;
        GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ReportGcMetrics();
                await Task.Delay(ReportInterval, stoppingToken)
                    .ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
            }
        }
        finally
        {
            GCSettings.LatencyMode = previous;
            logger.LogInformation("MetricsCollectorWorker stopped — GC latency mode restored to {Mode}", previous);
        }
    }

    private void ReportGcMetrics()
    {
        GCMemoryInfo info = GC.GetGCMemoryInfo();
        long totalMemory = GC.GetTotalMemory(forceFullCollection: false);

        logger.LogInformation(
            "Total memory: {TotalMemory:F1} MB | GC Metrics — HeapSize: {HeapMB:F1} MB | Fragmented: {FragKB:F0} KB | " +
            "MemLoad: {LoadMB:F0} MB | Gen0: {G0} | Gen1: {G1} | Gen2: {G2}",
            totalMemory / (1024.0 * 1024),
            info.HeapSizeBytes / (1024.0 * 1024),
            info.FragmentedBytes / 1024.0,
            info.MemoryLoadBytes / (1024.0 * 1024),
            GC.CollectionCount(0),
            GC.CollectionCount(1),
            GC.CollectionCount(2));

        // Alert if heap fragmentation exceeds 20 % — a sign of pinning or
        // improper ArrayPool usage creating holes in the managed heap.
        const long minHeapSizeForFragmentationWarningBytes = 50L * 1024 * 1024;
        if (info.HeapSizeBytes > minHeapSizeForFragmentationWarningBytes)
        {
            double fragmentationRatio = (double)info.FragmentedBytes / info.HeapSizeBytes;
            if (fragmentationRatio > 0.20)
            {
                logger.LogWarning(
                    "High heap fragmentation detected: {Ratio:P0} — review pinned buffers and ArrayPool usage",
                    fragmentationRatio);
            }
        }
    }
}
