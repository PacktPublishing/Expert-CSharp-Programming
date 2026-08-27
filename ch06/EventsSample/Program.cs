using EventsSample;

Observer observer = new();

Subject[] subjects = [.. Enumerable.Range(1, 100)
    .Select(x =>
    {
        Subject s = new(x);
        s.MyEvent += observer.Handler;
        return s;
    })];

foreach (var subject in subjects)
{
    subject.RaiseEvent();
}

for (int i = 0; i < subjects.Length; i++)
{
    subjects[i].MyEvent -= observer.Handler;
}
