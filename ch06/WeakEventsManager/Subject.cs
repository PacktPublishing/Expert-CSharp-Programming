using System.Windows.Media;

namespace WeakEvents;

public class Subject(int id)
{
    public int Id { get; } = id;
    private string _text = $"Hello, World! {id}";
    //private int[] _data = [.. Enumerable.Range(0, 100).Select(x => x)];

    public event EventHandler<SubjectEventArgs>? SomeEvent;

    public void RaiseEvent()
    {
        SomeEvent?.Invoke(this, new SubjectEventArgs(Id));
    }
}

public class SubjectEventArgs(int id) : EventArgs
{
    public int Id { get; } = id;
}
