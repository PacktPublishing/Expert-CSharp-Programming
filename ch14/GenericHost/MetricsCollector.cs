// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

namespace GenericHost;

// ─────────────────────────────────────────────────────────────────────────
// Supporting abstractions
// ─────────────────────────────────────────────────────────────────────────

public interface IMetricsCollector
{
    void RecordHeartbeat();
    int HeartbeatCount { get; }
}

/// <summary>Thread-safe in-memory metrics store.</summary>
public sealed class InMemoryMetricsCollector : IMetricsCollector
{
    private int _heartbeatCount;

    // Interlocked ensures thread-safe writes; Volatile.Read ensures
    // the freshest value is visible across threads without locking.
    public void RecordHeartbeat() => Interlocked.Increment(ref _heartbeatCount);
    public int HeartbeatCount => Volatile.Read(ref _heartbeatCount);
}
