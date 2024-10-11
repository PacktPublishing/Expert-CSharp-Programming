using GenericCollection;

int size = 10;
CustomCollection<int> data = new(size);

for (int i = 0; i < size; i++)
{
    data[i] = i;
}

foreach (var item in data)
{
    Console.WriteLine(item);
}

