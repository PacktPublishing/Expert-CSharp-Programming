using Synchronization;

await Runner.ReaderWriterLockSlimAsync();
await Runner.InterlockedAsync();
await Runner.SemaphoreSlimAsync();
await Runner.ReaderWriterLockSlimAsync();
await Runner.AvoidDeadlockAsync();
