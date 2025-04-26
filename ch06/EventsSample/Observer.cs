namespace EventsSample;

public class Observer
{
    public void Handler(object? sender, SubjectEventArgs args)
    {
        Console.WriteLine($"Received event from {args.Id}");
    }
}
