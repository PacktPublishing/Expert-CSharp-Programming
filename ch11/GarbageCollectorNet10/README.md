# GarbageCollectorNet10 — .NET 10 GC Configuration Sample

This sample demonstrates **garbage collector configuration and monitoring** features in .NET 10, including dynamic heap adaptation (DATAS), latency modes, no-GC regions, and memory inspection APIs.

## 🚀 Features Demonstrated

### 1. Current GC Settings
Read the active GC configuration at runtime:
```csharp
Console.WriteLine($"Is Server GC: {GCSettings.IsServerGC}");
Console.WriteLine($"Latency Mode: {GCSettings.LatencyMode}");
Console.WriteLine($"Max Generation: {GC.MaxGeneration}");
```

### 2. LowLatency Mode — Critical Section
Temporarily reduce GC pauses for a time-sensitive block:
```csharp
GCLatencyMode previousMode = GCSettings.LatencyMode;
GCSettings.LatencyMode = GCLatencyMode.LowLatency;
try
{
    AllocateWithArrayPool();
}
finally
{
    GCSettings.LatencyMode = previousMode;
}
```

| Mode | Description | Use Case |
|------|-------------|----------|
| `Batch` | Highest throughput, large pauses | Offline batch processing |
| `Interactive` | Balanced (default) | Most applications |
| `LowLatency` | Minimizes pauses, avoids Gen 2 | Real-time, UI responsiveness |
| `SustainedLowLatency` | Like LowLatency but sustained | Continuous low-latency scenarios |
| `NoGCRegion` | No GC during critical section | Short critical paths |

### 3. NoGCRegion — Zero-Pause Path
Suppress GC entirely during a sub-millisecond operation:
```csharp
if (GC.TryStartNoGCRegion(4 * 1024 * 1024))
{
    try { ExecuteCriticalOperation(); }
    finally { GC.EndNoGCRegion(); }
}
```

### 4. Collection Counts and Memory Measurement
Measure memory and count collections per generation:
```csharp
long before = GC.GetTotalMemory(forceFullCollection: false);
AllocateManyObjects();
long after = GC.GetTotalMemory(forceFullCollection: true);
Console.WriteLine($"Memory delta: {after - before} bytes");

for (int gen = 0; gen <= GC.MaxGeneration; gen++)
    Console.WriteLine($"Gen {gen} collections: {GC.CollectionCount(gen)}");
```

### 5. GCMemoryInfo — Heap Details
Inspect heap statistics:
```csharp
GCMemoryInfo memInfo = GC.GetGCMemoryInfo();
Console.WriteLine($"Heap size:      {memInfo.HeapSizeBytes:N0} bytes");
Console.WriteLine($"Fragmented:     {memInfo.FragmentedBytes:N0} bytes");
Console.WriteLine($"Memory load:    {memInfo.MemoryLoadBytes:N0} bytes");
```

### 6. ArrayPool — Reduce GC Pressure
Reuse buffers to minimise allocations in hot paths:
```csharp
byte[] buffer = ArrayPool<byte>.Shared.Rent(64_000);
try { /* use buffer */ }
finally { ArrayPool<byte>.Shared.Return(buffer); }
```

## 📋 Prerequisites

- **.NET 10 SDK** (10.0.101 or later)
- **Visual Studio 2026** or **VS Code** with C# extension

## 🏗️ Project Structure

```
GarbageCollectorNet10/
├── Program.cs                      # GC demo code
├── GarbageCollectorNet10.csproj    # Project file targeting net10.0
└── README.md                       # This file
```

## 🔧 GC Configuration Reference

### .csproj Properties
```xml
<PropertyGroup>
  <ServerGarbageCollection>true</ServerGarbageCollection>
  <GarbageCollectionAdaptationMode>1</GarbageCollectionAdaptationMode>
</PropertyGroup>
```

### runtimeconfig.json
```json
{
  "configProperties": {
    "System.GC.Server": true,
    "System.GC.DynamicAdaptationMode": 1,
    "System.GC.HeapHardLimit": 209715200
  }
}
```

### Environment Variables
```bash
DOTNET_GCServer=1                   # Enable Server GC
DOTNET_GCDynamicAdaptationMode=1    # Enable DATAS (default in .NET 10)
DOTNET_GCHeapHardLimit=209715200    # 200 MB hard limit
DOTNET_GCConserveMemory=5           # 0–9 conservation aggressiveness
DOTNET_GCHighMemPercent=90          # % physical memory before aggressive GC
```

## 🔧 Setup and Running

```bash
# Navigate to the project
cd src/GarbageCollectorNet10

# Restore and build
dotnet restore
dotnet build

# Run the demo
dotnet run
```

## 💡 .NET 10 GC Highlights

- **Dynamic Heap Adjustment (DATAS)**: Server GC dynamically adjusts heap count at runtime based on load — enabled by default via `GCDynamicAdaptationMode=1`
- **Improved LOH Compaction**: The Large Object Heap is compacted more efficiently
- **Reduced Pinning Overhead**: Better handling of pinned objects lowers fragmentation
- **Container Awareness**: .NET 10 auto-respects container memory limits; `GCHeapHardLimit` defaults to 75% of the container memory limit

## 📊 Sample Output

```
🚀 .NET 10 Garbage Collector Configuration Demo
================================================

1️⃣  Current GC Settings
------------------------
   Is Server GC:   False
   Latency Mode:   Interactive
   Max Generation: 2
   Total Memory:         66,112 bytes
   Total Alloc:          65,992 bytes

2️⃣  LowLatency Mode — Critical Section
---------------------------------------
   Switching to LowLatency mode to minimize GC pauses...
   Latency mode during critical section: LowLatency
   Latency mode restored to: Interactive

3️⃣  NoGCRegion — Zero-Pause Critical Path
------------------------------------------
   No-GC region started and ended — GC pauses were suppressed.

4️⃣  Collection Counts and Memory Measurement
---------------------------------------------
   Memory before allocations:   14,093,352 bytes
   Memory after full GC:         4,320,520 bytes
   Memory delta:                -9,772,832 bytes

   GC Collection Counts per Generation:
   Gen 0:   44 collections
   Gen 1:   22 collections
   Gen 2:    3 collections

5️⃣  GCMemoryInfo — Heap Details
--------------------------------
   Heap size bytes:              4,321,592
   Fragmented bytes:                 1,072
   Memory load bytes:         2,165,596,160
   Total available memory:    8,329,216,000
   High memory load threshold:7,496,294,400

6️⃣  .NET 10 GC Configuration Reference
----------------------------------------
   [configuration snippets printed to console]

✅ Demo Complete!
```

## 🎯 Best Practices

- **Default settings are well-tuned** — only customize after profiling
- **Server GC** is automatically enabled in ASP.NET Core
- **Set `GCHeapHardLimit`** in containers to prevent OOM kills
- **Enable DATAS** (`GCDynamicAdaptationMode=1`) for variable-load server workloads
- **Avoid `GC.Collect()`** in production — let the runtime decide
- **Use `ArrayPool<T>`** for frequently allocated buffers to reduce GC pressure

## 📚 Additional Resources

- [Garbage Collection in .NET](https://docs.microsoft.com/dotnet/standard/garbage-collection/)
- [GCSettings Class](https://docs.microsoft.com/dotnet/api/system.runtime.gcsettings)
- [GC.GetGCMemoryInfo](https://docs.microsoft.com/dotnet/api/system.gc.getgcmemoryinfo)
- [Runtime Configuration — GC](https://docs.microsoft.com/dotnet/core/runtime-config/garbage-collector)
- [slides/garbage-collector.md](../../slides/garbage-collector.md) — Background and diagrams
