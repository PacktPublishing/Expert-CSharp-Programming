int[] data = [1, 2, 3, 4, 5, 6, 7];

Func<Span<int>, Span<int>> incrementFirstThree = (Span<int> s) =>
{
    var slice = s[0..3];
    for (int i = 0; i < slice.Length; i++)
    {
        slice[i]++;
    }
    return s;
};

incrementFirstThree(data.AsSpan());

foreach (var item in data)
{
    Console.WriteLine(item);
}   
