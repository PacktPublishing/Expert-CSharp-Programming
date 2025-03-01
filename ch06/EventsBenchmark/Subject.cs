using Microsoft.Diagnostics.Tracing.Parsers;

namespace WeakEvents;

public class Subject(int id)
{
    public int Id { get; } = id;
    private string _text = $"Hello, World! {id}";   
    private byte[] data = new byte[1024];

    public event EventHandler<SubjectEventArgs>? SomeEvent;

    public void RaiseEvent()
    {
        SomeEvent?.Invoke(this, new SubjectEventArgs(Id));
    }

    public override string ToString() => $"Subject {Id}";
}

public class Subject2(int id)
{
    public int Id { get; } = id;
    private string _text = $"Hello, World! {id}";
    private byte[] data = new byte[1024];

    private WeakEvent<SubjectEventArgs> _weakEvent = new();

    public event EventHandler<SubjectEventArgs> SomeEvent
    {
        add
        {
            _weakEvent.AddHandler(value);
        }
        remove
        {
            _weakEvent.RemoveHandler(value);
        }
    }

    public void RaiseEvent()
    {
        _weakEvent.RaiseEvent(this, new SubjectEventArgs(Id));
    }

    public override string ToString() => $"Subject {Id}";
}

public class SubjectEventArgs(int id) : EventArgs
{
    public int Id { get; } = id;
}
