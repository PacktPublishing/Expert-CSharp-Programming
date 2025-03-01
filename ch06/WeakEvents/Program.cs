
using WeakEvents;

Console.WriteLine(GC.GetTotalMemory(true));
Observer observer = new();

var subjects = Enumerable.Range(1, 1000)
    .Select(x =>
    {
        Subject s = new(x);
        s.SomeEvent += observer.Handler;
        return s;
    })
    .ToArray();


foreach (var subject in subjects)
{
    subject.RaiseEvent();
}

GC.Collect();

Console.WriteLine(GC.GetTotalMemory(true));


for (int i = 0; i < subjects.Length; i++)
{
    subjects[i] = null!;
}

subjects = null;
GC.Collect();
GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive);
GC.WaitForFullGCComplete();

await Task.Delay(1000);

GC.Collect();
GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive);
GC.WaitForFullGCComplete();

GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive);
GC.WaitForFullGCComplete();

GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive);
GC.WaitForFullGCComplete();

Console.WriteLine(GC.GetTotalMemory(true));

//foreach (var subject in subjects)
//{
//    Console.WriteLine(subject.ToString());
//}

Console.WriteLine("End");
