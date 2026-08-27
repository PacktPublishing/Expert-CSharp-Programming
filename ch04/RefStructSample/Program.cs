RefStruct rs = new(42);

//DoFoo(rs); // This does not compile
DoFooGeneric(rs); // This works

static void DoFoo(IFoo foo)
{
    foo.Foo();
}

static void DoFooGeneric<T>(T foo) where T : IFoo, allows ref struct
{
    foo.Foo();
}


public interface IFoo
{
    void Foo();
}

public ref struct RefStruct(int value) : IFoo
{
    public int Value = value;

    public void Foo()
    {
        Console.WriteLine($"Value: {Value}");
    }
}