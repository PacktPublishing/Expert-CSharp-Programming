using ThreadsAndTasks;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Runner.ThreadSample();
await Runner.TaskContinuations();
await Runner.MultipleTasksAsync();
await Runner.FirstResponseWinsAsync();
await Runner.CancellationAsync();
