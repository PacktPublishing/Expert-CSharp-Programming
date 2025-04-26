
var tasks = new Task[5];

for (int i = 0; i < 5; i++)
{
    tasks[i] = Task.Run(() =>
    {
        Console.Write($"{i}\t");
    });
}
await Task.WhenAll(tasks);
Console.WriteLine();

Console.WriteLine("\nusing local variable");
for (int i = 0; i < 5; i++)
{
    int val = i;
    tasks[i] = Task.Run(() =>
    {
        Console.Write($"{val}\t");
    });
}
await Task.WhenAll(tasks);
Console.WriteLine();
