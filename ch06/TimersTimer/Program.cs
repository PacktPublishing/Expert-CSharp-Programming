using TimersTimer = System.Timers.Timer;

using TimersTimer timer = new(TimeSpan.FromSeconds(2));
timer.Elapsed += static (sender, e) =>
{
    Console.WriteLine($"Timer Event: {e.SignalTime:hh:mm:ss}");
};
timer.Start();

await Task.Delay(6000);