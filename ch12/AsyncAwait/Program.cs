using AsyncAwait;

Console.OutputEncoding = System.Text.Encoding.UTF8;

await Runner.AsyncAndAwaitAsync();
await Runner.ValueTaskSampleAsync();
await Runner.ConfigureAwaitSampleAsync();
await Runner.AsyncStreaming();
