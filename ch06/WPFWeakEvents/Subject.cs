namespace WPFWeakEvents;

public class Subject(int id)
{
    public int Id { get; } = id;

    public event EventHandler<SubjectEventArgs>? SomeEvent;

    public void RaiseEvent()
    {
        SomeEvent?.Invoke(this, new SubjectEventArgs(Id));
    }
}
