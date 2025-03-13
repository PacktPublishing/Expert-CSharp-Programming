namespace WPFWeakEvents;

public class Observer(int id)
{
    public int Id { get; } = id;
    public void Handler(object? sender, SubjectEventArgs args)
    {
        Console.WriteLine($"Received event from {args.Id}");
    }
}
