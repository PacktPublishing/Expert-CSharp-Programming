
//using Timer timer = new(OnTimer, null, dueTime: 2000, period: 500);
//Console.ReadLine();

//void OnTimer(object? state)
//{
//    Console.WriteLine("TimerCallback: " + DateTime.Now);
//}

using Timer = System.Timers;

PeriodicTimer timer = new(OnTimer, null, dueTime: 2000, period: 500);
timer.
System.Timers.Timer timer = new(2000);