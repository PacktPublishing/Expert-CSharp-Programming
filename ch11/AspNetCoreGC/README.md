# AspNetCoreGC — GC Patterns for ASP.NET Core

Demonstrates **production-ready GC patterns** for ASP.NET Core Minimal API applications, including memory diagnostics, `ObjectPool<StringBuilder>`, and `ArrayPool<byte>` for handling large request payloads without LOH pressure.

## 🚀 Endpoints

| Endpoint | Description |
|----------|-------------|
| `GET /gc/stats` | Live GC memory info (heap size, fragmentation, collection counts) |
| `GET /gc/pressure` | Allocates 10 000 short-lived arrays to trigger Gen-0/Gen-1 GC |
| `GET /gc/pool` | Builds a response using a pooled `StringBuilder` — avoids per-request allocation of the builder itself |
| `GET /gc/arraybuffer` | Processes a 100 KB buffer via `ArrayPool<byte>` — avoids LOH allocation |

## 🎓 Skills Demonstrated

### Skill 1 — ASP.NET Core enables Server GC automatically
```csharp
ILogger startupLogger = app.Logger;
startupLogger.LogInformation(
    "GC mode: IsServerGC={IsServer} | LatencyMode={Mode}",
    GCSettings.IsServerGC,     // true in ASP.NET Core
    GCSettings.LatencyMode);   // Interactive by default
```
ASP.NET Core sets `ServerGarbageCollection=true` in its hosting layer. You do not need to configure it manually unless running as a Worker.

### Skill 2 — Expose a memory-diagnostic health endpoint
```csharp
app.MapGet("/gc/stats", () =>
{
    GCMemoryInfo info = GC.GetGCMemoryInfo();
    return new GcStats(
        HeapSizeMB:   info.HeapSizeBytes / (1024.0 * 1024),
        FragmentedMB: info.FragmentedBytes / (1024.0 * 1024),
        Gen0Collections: GC.CollectionCount(0), ...);
});
```
Integrate this into `/healthz` or a Prometheus scrape endpoint to monitor GC health without a profiler.

### Skill 3 — Use ObjectPool\<StringBuilder\> for per-request string building
```csharp
builder.Services.AddSingleton<ObjectPool<StringBuilder>>(sp =>
    sp.GetRequiredService<ObjectPoolProvider>()
      .CreateStringBuilderPool(initialCapacity: 512, maximumRetainedCapacity: 4096));

app.MapGet("/gc/pool", (ObjectPool<StringBuilder> pool) =>
{
    StringBuilder sb = pool.Get();
    try
    {
        sb.Append("...");
        return sb.ToString();
    }
    finally { pool.Return(sb); }  // clears and returns — zero Gen-0 allocation
});
```

### Skill 4 — Use ArrayPool\<byte\> for large request/response buffers
```csharp
app.MapGet("/gc/arraybuffer", () =>
{
    byte[] buffer = ArrayPool<byte>.Shared.Rent(100 * 1024);  // > 85 KB LOH threshold → LOH without pool
    try
    {
        // process buffer...
        return $"Processed {buffer.Length / 1024} KB — no LOH allocation.";
    }
    finally { ArrayPool<byte>.Shared.Return(buffer); }
});
```

## 🏗️ Project Structure

```
AspNetCoreGC/
├── Program.cs              # All endpoints and DI setup
├── AspNetCoreGC.csproj     # net10.0 Web SDK project
└── README.md               # This file
```

## 🔧 GC Configuration for ASP.NET Core

ASP.NET Core sets the following automatically at host startup:

```json
// runtimeconfig.json equivalent
{
  "System.GC.Server": true,
  "System.GC.Concurrent": true
}
```

For containers, add a memory limit:
```bash
DOTNET_GCHeapHardLimit=209715200   # 200 MB hard cap
DOTNET_GCDynamicAdaptationMode=1   # DATAS — shrinks heap count under low load
```

For high-traffic APIs with spiky load:
```bash
DOTNET_GCConserveMemory=5          # 0–9: aggressiveness of memory return to OS
DOTNET_GCHighMemPercent=85         # trigger aggressive GC above 85% physical memory
```

## ▶️ Running the Sample

```bash
cd ch11/AspNetCoreGC
dotnet run

# In another terminal:
curl http://localhost:5000/gc/stats        # view heap metrics
curl http://localhost:5000/gc/pressure     # add GC pressure
curl http://localhost:5000/gc/stats        # see updated collection counts
curl http://localhost:5000/gc/pool         # pooled StringBuilder response
curl http://localhost:5000/gc/arraybuffer  # ArrayPool buffer processing
```

## 📊 Sample /gc/stats Response

```json
{
  "isServerGC": true,
  "latencyMode": "Interactive",
  "heapSizeMB": 4.12,
  "fragmentedMB": 0.0,
  "memoryLoadMB": 2048.5,
  "totalAvailableMemoryMB": 8192.0,
  "gen0Collections": 12,
  "gen1Collections": 3,
  "gen2Collections": 0,
  "totalAllocatedBytesMB": 25.3
}
```

## 💡 Key Takeaways

- ASP.NET Core enables Server GC automatically — verify with `GCSettings.IsServerGC`
- Expose a `/gc/stats` endpoint to monitor heap health in production
- `ObjectPool<StringBuilder>` avoids allocating a new builder per request; individual Append calls are used to avoid per-item interpolated string allocations
- Buffers > 85 KB go to the **Large Object Heap (LOH)** — always use `ArrayPool<byte>` for them
- Set `DOTNET_GCHeapHardLimit` in containers to prevent OOM kills from uncapped heap growth
