using Synchronization;

Console.OutputEncoding = System.Text.Encoding.UTF8;
await Runner.LockKeywordAsync();
await Runner.InterlockedAsync();
await Runner.SemaphoreSlimAsync();
await Runner.ReaderWriterLockSlimAsync();
await Runner.AvoidDeadlockAsync();
