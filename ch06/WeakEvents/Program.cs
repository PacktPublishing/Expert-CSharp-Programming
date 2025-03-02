using WeakEvents;

const int numberObservers = 10;
const int numberSubjects = 10;

Observer?[]? observers = [.. Enumerable.Range(1, numberObservers).Select(x => new Observer(x))];

Subject[] subjects = [.. Enumerable.Range(1, numberSubjects)
    .Select(x =>
    {
        Subject s = new(x);
        foreach (var observer in observers)
        {
            s.SomeEvent += observer!.Handler; // at this time, all observers are not null
        }
        return s;
    })];

foreach (var subject in subjects)
{
    subject.RaiseEvent();
}


for (int i = 0; i < observers.Length; i++)
{
    observers[i] = null;
}

Console.WriteLine("End");
