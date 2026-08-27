# BackgroundServiceGC — GC Patterns for .NET Background Services

Demonstrates how **Garbage Collector behavior** differs in long-running **.NET Worker services** and which patterns to apply to keep memory usage predictable in production.

## 🚀 What's Inside

| File | Purpose |
|------|---------|
| `Program.cs` | Host setup, DI registration of workers and shared pool |
| `MessageBufferPool.cs` | Custom `ArrayPool<byte>` wrapper for reusable message buffers |
| `MetricsCollectorWorker.cs` | Periodic GC health reporter using `GCMemoryInfo` |
| `DataProcessingWorker.cs` | Side-by-side comparison: `new byte[]` vs `ArrayPool` allocation |

## 🎓 Skills Demonstrated

### Skill 1 — Configure GC latency for long-running services
```csharp
GCLatencyMode previous = GCSettings.LatencyMode;
GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
try
{
    // hot path — Gen-2 collections will be suppressed
}
finally
{
    GCSettings.LatencyMode = previous;
}
```
`SustainedLowLatency` keeps Gen-2 collections from pausing real-time processing threads.

### Skill 2 — Monitor GC health from inside a worker
```csharp
GCMemoryInfo info = GC.GetGCMemoryInfo();
logger.LogInformation("Heap: {MB} MB | Fragmented: {KB} KB", 
    info.HeapSizeBytes / (1024.0 * 1024),
    info.FragmentedBytes / 1024.0);

double ratio = (double)info.FragmentedBytes / info.HeapSizeBytes;
if (ratio > 0.20)
    logger.LogWarning("High fragmentation {Ratio:P0} — review pinned buffers", ratio);
```

### Skill 3 — Use ArrayPool to eliminate Gen-2 allocations
```csharp
// ❌ Anti-pattern: new byte[] lands in LOH for buffers ≥ 85 KB
byte[] buffer = new byte[MessageSizeBytes];

// ✅ Fix: rent from the pool — no new Gen-0 object
byte[] buffer = bufferPool.Rent(MessageSizeBytes);
try { ProcessMessage(buffer); }
finally { bufferPool.Return(buffer, clearArray: true); }
```

### Skill 4 — Use `ConfigureAwait(SuppressThrowing)` in workers
```csharp
await Task.Delay(interval, token).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
```
Avoids `OperationCanceledException` allocation when the token is cancelled during a delay — a small but zero-cost improvement in high-throughput workers.

## 🏗️ Project Structure

```
BackgroundServiceGC/
├── Program.cs                  # Host and DI setup
├── MessageBufferPool.cs        # Shared ArrayPool<byte> wrapper
├── MetricsCollectorWorker.cs   # Periodic GC metrics reporter
├── DataProcessingWorker.cs     # Classic new[] vs. ArrayPool comparison
├── BackgroundServiceGC.csproj  # net10.0 Worker SDK project
└── README.md                   # This file
```

## 🔧 GC Configuration for Worker Services

### Enable Server GC for CPU-bound background services
```xml
<!-- .csproj -->
<ServerGarbageCollection>true</ServerGarbageCollection>
<GarbageCollectionAdaptationMode>1</GarbageCollectionAdaptationMode>
```

| Setting | Workstation GC (default) | Server GC |
|---------|--------------------------|-----------|
| Heaps | 1 | 1 per logical core |
| Background GC thread | 1 | 1 per heap |
| Throughput | Moderate | High |
| Pause per Gen-2 | ~1–10 ms | ~1–10 ms per heap |
| Best for | Client / single-core | Multi-core server |

### Container memory limit
```bash
DOTNET_GCHeapHardLimit=209715200   # 200 MB — prevent OOM in containers
DOTNET_GCDynamicAdaptationMode=1   # DATAS: shrink heap count when idle
```

## ▶️ Running the Sample

```bash
cd ch11/BackgroundServiceGC
dotnet run
```

The `DataProcessingWorker` runs for 10 seconds processing messages both ways, then logs a comparison of GC collection counts.

## 📊 Expected Output (abbreviated)

```
info: DataProcessingWorker  — Started
info: MetricsCollectorWorker — GC Metrics — HeapSize: 4.2 MB | Fragmented: 0 KB | Gen0: 5 | Gen1: 1 | Gen2: 0
info: DataProcessingWorker  — Comparison after 10s — Allocating: 81.0 MB processed | Pooling: 81.0 MB processed | Gen0 so far: 120 | Gen1 so far: 18
info: DataProcessingWorker  — Takeaway: the pooling path should show significantly fewer GC collections...
```

## 💡 Key Takeaways

- `SustainedLowLatency` prevents Gen-2 pauses — use it on threads processing real-time data
- `GCMemoryInfo.FragmentedBytes / HeapSizeBytes > 0.20` is a warning sign of excessive pinning
- `ArrayPool<T>` eliminates repeated allocations in high-throughput hot paths
- Always return rented buffers in a `finally` block — leaking rented buffers exhausts the pool
