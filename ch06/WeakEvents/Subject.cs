namespace WeakEvents;

public class Subject(int id)
{
    public int Id { get; } = id;

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
}
