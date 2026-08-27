using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

// Uncomment the following line to run the benchmark
// BenchmarkRunner.Run<BenchmarkSample2>();

// use a value type
int x1 = 42;
int x2 = (int)Sample1.PassAnObject(x1);
x2 = Sample1.PassAnyType(x1);

// use a reference type
string s1 = "Hello";
string s2 = (string)Sample1.PassAnObject(s1);
s2 = Sample1.PassAnyType(s1);

public class Sample1
{
    public static object PassAnObject(object o) => o;
    public static T PassAnyType<T>(T t) => t;
}

[MemoryDiagnoser]
public class BenchmarkSample2
{
    private readonly MyStruct _myStruct = new(1, 2);

    [Benchmark]
    public void PassStructWithAnObject()
    {
        Sample2.PassAnObject(_myStruct);
    }
    [Benchmark]
    public void PassStructGeneric()
    {
        Sample2.PassAGeneric(_myStruct);
    }

    private readonly MyClass _myClass = new(1, 2);

    [Benchmark]
    public void PassClassWithAnObject()
    {
        Sample2.PassAnObject(_myClass);
    }
    [Benchmark]
    public void PassClassGeneric()
    {
        Sample2.PassAGeneric(_myClass);
    }
}