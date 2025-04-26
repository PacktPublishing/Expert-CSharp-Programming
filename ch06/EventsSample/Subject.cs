namespace EventsSample;

public class Subject(int id)
{
    public int Id { get; } = id;

    public event EventHandler<SubjectEventArgs>? MyEvent;

    public void RaiseEvent()
    {
        MyEvent?.Invoke(this, new SubjectEventArgs(Id));
    }

    public override string ToString() => $"Subject {Id}";
}
