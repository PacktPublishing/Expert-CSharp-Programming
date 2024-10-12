int x = 42;
PassAnObject(x);
PassAnyType(x);

string s = "Hello";
PassAnObject(s);
PassAnyType(s);

static void PassAnObject(object o)
{
    Console.WriteLine(o);
}

static void PassAnyType<T>(T t)
{
    Console.WriteLine(t);
}
