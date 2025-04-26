using System.Runtime.InteropServices.Marshalling;

namespace WeakEvents;

public class Observer(int id)
{
    public int Id { get; } = id;

    public void Handler(object? sender, SubjectEventArgs args)
    {
        Console.WriteLine($"Observer {Id} received event from subject {args.Id}");
    }
}
