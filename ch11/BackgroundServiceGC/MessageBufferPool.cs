using System.Buffers;

namespace BackgroundServiceGC;

// ============================================================
// MessageBufferPool — Shared ArrayPool wrapper
// Demonstrates how pooling large byte arrays prevents repeated
// Gen-2 / LOH allocations in long-running background services.
// ============================================================
public sealed class MessageBufferPool
{
    private readonly ArrayPool<byte> _pool = ArrayPool<byte>.Create(
        maxArrayLength: 256 * 1024,  // 256 KB max per buffer
        maxArraysPerBucket: 10);     // Keep up to 10 idle per size class

    public byte[] Rent(int minimumLength) => _pool.Rent(minimumLength);

    public void Return(byte[] buffer, bool clearArray = false) =>
        _pool.Return(buffer, clearArray);
}
