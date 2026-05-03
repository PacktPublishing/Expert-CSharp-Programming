// ============================================================
// AspNetCoreGC — Production GC Patterns for ASP.NET Core
//
// ASP.NET Core automatically enables Server GC (one heap per
// logical core) to maximise throughput on multi-core hosts.
// This sample shows:
//
//  1. How to read and log the active GC configuration on startup
//  2. A memory-diagnostic endpoint returning live GC stats
//  3. An ObjectPool-backed endpoint that avoids per-request
//     heap allocations for temporary StringBuilder work
//  4. A "pressure" endpoint that allocates many short-lived objects
//     so you can observe Gen0/Gen1 GC activity via /gc/stats
//
// Run with:
//   dotnet run
// Then browse to:
//   GET /gc/stats    → live GC memory info
//   GET /gc/pressure → allocate short-lived objects (demo)
//   GET /gc/pool     → build a report using ObjectPool<StringBuilder>
// ============================================================

using System.Buffers;
using System.Runtime;
using System.Text;
using Microsoft.Extensions.ObjectPool;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ── Register ObjectPool<StringBuilder> ────────────────────────────────────────
// ObjectPool keeps a pool of pre-allocated StringBuilders so that high-traffic
// endpoints never allocate a new one per request.  The pool is bounded: extra
// instances beyond the pool size are simply dropped (collected by GC).
builder.Services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

builder.Services.AddSingleton<ObjectPool<StringBuilder>>(sp =>
{
    ObjectPoolProvider provider = sp.GetRequiredService<ObjectPoolProvider>();
    // StringBuilderPooledObjectPolicy clears the builder before returning it
   
    return provider.CreateStringBuilderPool(initialCapacity: 512, maximumRetainedCapacity: 4096);
});

WebApplication app = builder.Build();

// ── Log GC configuration on startup ───────────────────────────────────────────
ILogger startupLogger = app.Logger;
startupLogger.LogInformation(
    "GC mode: IsServerGC={IsServer} | LatencyMode={Mode} | MaxGeneration={MaxGen}",
    GCSettings.IsServerGC,
    GCSettings.LatencyMode,
    GC.MaxGeneration);

// ── Endpoints ─────────────────────────────────────────────────────────────────

// 1. Live GC diagnostic endpoint — suitable for a /healthz or metrics sidecar
app.MapGet("/gc/stats", () =>
{
    GCMemoryInfo info = GC.GetGCMemoryInfo();

    return new GcStats(
        IsServerGC:               GCSettings.IsServerGC,
        LatencyMode:              GCSettings.LatencyMode.ToString(),
        HeapSizeMB:               Math.Round(info.HeapSizeBytes / (1024.0 * 1024), 2),
        FragmentedMB:             Math.Round(info.FragmentedBytes / (1024.0 * 1024), 2),
        MemoryLoadMB:             Math.Round(info.MemoryLoadBytes / (1024.0 * 1024), 2),
        TotalAvailableMemoryMB:   Math.Round(info.TotalAvailableMemoryBytes / (1024.0 * 1024), 2),
        Gen0Collections:          GC.CollectionCount(0),
        Gen1Collections:          GC.CollectionCount(1),
        Gen2Collections:          GC.CollectionCount(2),
        TotalAllocatedBytesMB:    Math.Round(GC.GetTotalAllocatedBytes(precise: false) / (1024.0 * 1024), 2));
});

// 2. Pressure endpoint — allocate 10 000 short-lived arrays to trigger GC
//    Compare Gen0/Gen1 counts before and after to see the impact.
app.MapGet("/gc/pressure", () =>
{
    int gen0Before = GC.CollectionCount(0);
    int gen1Before = GC.CollectionCount(1);

    for (int i = 0; i < 10_000; i++)
    {
        // ❌ Anti-pattern: allocating a fresh array inside a hot path
        byte[] temporary = new byte[1_024];
        temporary[0] = (byte)(i & 0xFF); // prevent elimination
        _ = temporary;
    }

    return new PressureResult(
        Gen0Delta: GC.CollectionCount(0) - gen0Before,
        Gen1Delta: GC.CollectionCount(1) - gen1Before,
        Message:   "Allocated 10 000 × 1 KB arrays. " +
                   "Call /gc/stats to see the updated heap metrics.");
});

// 3. Pool endpoint — uses ObjectPool<StringBuilder> to build a JSON-like report
//    without allocating a new StringBuilder per request.
app.MapGet("/gc/pool", (ObjectPool<StringBuilder> pool) =>
{
    StringBuilder sb = pool.Get();
    try
    {
        sb.Append("{ \"entries\": [");
        for (int i = 0; i < 20; i++)
        {
            if (i > 0) sb.Append(", ");
            // ✅ Use individual Append calls instead of $"\"item-{i}\"" to avoid
            //    an intermediate interpolated string allocation per iteration.
            sb.Append('"');
            sb.Append("item-");
            sb.Append(i);
            sb.Append('"');
        }
        sb.Append("] }");
        return sb.ToString();
    }
    finally
    {
        // Returning clears the builder and puts it back in the pool —
        // the next request reuses the same object without a Gen-0 allocation.
        pool.Return(sb);
    }
});

// 4. ArrayPool endpoint — shows renting / returning a large buffer from
//    the shared ArrayPool instead of allocating one on the heap.
app.MapGet("/gc/arraybuffer", () =>
{
    const int BufferSize = 100 * 1024; // 100 KB — exceeds the ~85 KB LOH threshold, so it would land on the LOH without pooling

    byte[] buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
    try
    {
        // Simulate filling the buffer with request-derived data
        for (int i = 0; i < BufferSize; i++)
            buffer[i] = (byte)(i & 0xFF);

        int checksum = 0;
        foreach (byte b in buffer.AsSpan(0, BufferSize))
            checksum ^= b;

        return $"Processed {BufferSize / 1024} KB buffer (checksum={checksum}) using ArrayPool — no LOH allocation.";
    }
    finally
    {
        ArrayPool<byte>.Shared.Return(buffer);
    }
});

app.Run();

// ── Response DTOs ──────────────────────────────────────────────────────────────
record GcStats(
    bool   IsServerGC,
    string LatencyMode,
    double HeapSizeMB,
    double FragmentedMB,
    double MemoryLoadMB,
    double TotalAvailableMemoryMB,
    int    Gen0Collections,
    int    Gen1Collections,
    int    Gen2Collections,
    double TotalAllocatedBytesMB);

record PressureResult(int Gen0Delta, int Gen1Delta, string Message);
