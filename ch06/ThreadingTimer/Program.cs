Timer timer = new(static (object? _) =>
{
    Console.WriteLine($"timer called at {DateTime.Now:HH:mm:ss}");
}, state: null, dueTime: 3000, period: 1500);

await Task.Delay(10000);
timer.Dispose();
