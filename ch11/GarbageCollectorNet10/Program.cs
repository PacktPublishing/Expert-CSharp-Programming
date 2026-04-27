using System.Buffers;
using System.Runtime;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("🚀 .NET 10 Garbage Collector Configuration Demo");
Console.WriteLine("================================================");
Console.WriteLine();

// ============================================================
// 1. Current GC Settings
// ============================================================
Console.WriteLine("1️⃣  Current GC Settings");
Console.WriteLine("------------------------");
Console.WriteLine($"   Is Server GC:   {GCSettings.IsServerGC}");
Console.WriteLine($"   Latency Mode:   {GCSettings.LatencyMode}");
Console.WriteLine($"   Max Generation: {GC.MaxGeneration}");
Console.WriteLine($"   Total Memory:   {GC.GetTotalMemory(forceFullCollection: false),12:N0} bytes");
Console.WriteLine($"   Total Alloc:    {GC.GetTotalAllocatedBytes(precise: false),12:N0} bytes");
Console.WriteLine();

// ============================================================
// 2. LowLatency Mode — Reduce GC pauses during a critical section
// ============================================================
Console.WriteLine("2️⃣  LowLatency Mode — Critical Section");
Console.WriteLine("---------------------------------------");
Console.WriteLine("   Switching to LowLatency mode to minimize GC pauses...");

GCLatencyMode previousMode = GCSettings.LatencyMode;
GCSettings.LatencyMode = GCLatencyMode.LowLatency;
try
{
    AllocateWithArrayPool();
    Console.WriteLine($"   Latency mode during critical section: {GCSettings.LatencyMode}");
}
finally
{
    GCSettings.LatencyMode = previousMode;
    Console.WriteLine($"   Latency mode restored to: {GCSettings.LatencyMode}");
}
Console.WriteLine();

// ============================================================
// 3. NoGCRegion — Guarantee zero GC during a time-critical path
// ============================================================
Console.WriteLine("3️⃣  NoGCRegion — Zero-Pause Critical Path");
Console.WriteLine("------------------------------------------");

const long ReservedBytes = 4 * 1024 * 1024; // Reserve 4 MB
bool noGCStarted = GC.TryStartNoGCRegion(ReservedBytes);
if (noGCStarted)
{
    try
    {
        // Keep the no-GC region free of allocations (no Console.WriteLine here)
        ExecuteCriticalOperation();
    }
    finally
    {
        try
        {
            GC.EndNoGCRegion();
        }
        catch (InvalidOperationException)
        {
            // GC was triggered inside the region due to allocation pressure
        }
    }
    Console.WriteLine("   No-GC region started and ended — GC pauses were suppressed.");
}
else
{
    Console.WriteLine("   ⚠️  Could not start No-GC region (insufficient memory reserved).");
}
Console.WriteLine();

// ============================================================
// 4. Collection Counts and Memory Measurement
// ============================================================
Console.WriteLine("4️⃣  Collection Counts and Memory Measurement");
Console.WriteLine("---------------------------------------------");

long memoryBefore = GC.GetTotalMemory(forceFullCollection: false);
AllocateManyObjects();
long memoryAfter = GC.GetTotalMemory(forceFullCollection: true);

Console.WriteLine($"   Memory before allocations: {memoryBefore,12:N0} bytes");
Console.WriteLine($"   Memory after full GC:      {memoryAfter,12:N0} bytes");
Console.WriteLine($"   Memory delta:              {memoryAfter - memoryBefore,12:N0} bytes");
Console.WriteLine();

Console.WriteLine("   GC Collection Counts per Generation:");
for (int gen = 0; gen <= GC.MaxGeneration; gen++)
{
    Console.WriteLine($"   Gen {gen}: {GC.CollectionCount(gen),4} collections");
}
Console.WriteLine();

// ============================================================
// 5. GCMemoryInfo — Inspect Heap Details (.NET 5+)
// ============================================================
Console.WriteLine("5️⃣  GCMemoryInfo — Heap Details");
Console.WriteLine("--------------------------------");

GCMemoryInfo memInfo = GC.GetGCMemoryInfo();
Console.WriteLine($"   Heap size bytes:           {memInfo.HeapSizeBytes,12:N0}");
Console.WriteLine($"   Fragmented bytes:          {memInfo.FragmentedBytes,12:N0}");
Console.WriteLine($"   Memory load bytes:         {memInfo.MemoryLoadBytes,12:N0}");
Console.WriteLine($"   Total available memory:    {memInfo.TotalAvailableMemoryBytes,12:N0}");
Console.WriteLine($"   High memory load threshold:{memInfo.HighMemoryLoadThresholdBytes,12:N0}");
Console.WriteLine();

// ============================================================
// 6. GC Configuration Summary (runtimeconfig.json / env vars)
// ============================================================
Console.WriteLine("6️⃣  .NET 10 GC Configuration Reference");
Console.WriteLine("----------------------------------------");
Console.WriteLine("   runtimeconfig.json:");
Console.WriteLine(@"   {
     ""configProperties"": {
       ""System.GC.Server"": true,
       ""System.GC.DynamicAdaptationMode"": 1,
       ""System.GC.HeapHardLimit"": 209715200
     }
   }");
Console.WriteLine();
Console.WriteLine("   Environment variables:");
Console.WriteLine("   DOTNET_GCServer=1");
Console.WriteLine("   DOTNET_GCDynamicAdaptationMode=1   # DATAS — default in .NET 10");
Console.WriteLine("   DOTNET_GCHeapHardLimit=209715200   # 200 MB hard limit");
Console.WriteLine("   DOTNET_GCConserveMemory=5          # 0–9 conservation level");
Console.WriteLine();
Console.WriteLine("   .csproj properties:");
Console.WriteLine("   <ServerGarbageCollection>true</ServerGarbageCollection>");
Console.WriteLine("   <GarbageCollectionAdaptationMode>1</GarbageCollectionAdaptationMode>");
Console.WriteLine();

Console.WriteLine("✅ Demo Complete!");
Console.WriteLine("=================");
Console.WriteLine("Key Takeaways:");
Console.WriteLine("• GCSettings.LatencyMode controls GC aggressiveness at runtime");
Console.WriteLine("• LowLatency mode minimizes Gen 2 pauses for real-time sections");
Console.WriteLine("• NoGCRegion suppresses GC entirely during sub-millisecond paths");
Console.WriteLine("• .NET 10 DATAS (GCDynamicAdaptationMode=1) auto-adjusts heap count");
Console.WriteLine("• ArrayPool<T> drastically reduces GC pressure in hot paths");

// ============================================================
// Helper methods
// ============================================================

static void AllocateWithArrayPool()
{
    const int totalBuffers = 1_000;
    const int batchSize = 100;

    List<byte[]> buffers = new(batchSize);
    int rented = 0;

    while (rented < totalBuffers)
    {
        buffers.Clear();
        int currentBatch = Math.Min(batchSize, totalBuffers - rented);

        for (int i = 0; i < currentBatch; i++)
        {
            buffers.Add(ArrayPool<byte>.Shared.Rent(64_000));
        }

        // Return current batch to the pool — minimal GC pressure
        foreach (byte[] buffer in buffers)
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }

        rented += currentBatch;
    }
}

static void ExecuteCriticalOperation()
{
    // Simulate a time-sensitive operation (e.g., order execution, frame render)
    double result = 0;
    for (int i = 0; i < 100_000; i++)
    {
        result += Math.Sqrt(i);
    }
    _ = result; // prevent optimization
}

static void AllocateManyObjects()
{
    // Allocate short-lived objects to trigger Gen 0/1 collections
    for (int i = 0; i < 50_000; i++)
    {
        _ = new byte[256];
    }
}
