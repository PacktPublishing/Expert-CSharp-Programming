
using System.Runtime.InteropServices;

namespace CollectionTypes;

public class StackSample : IShowTitle
{
    public static void Run()
    {
        IShowTitle.ShowTitle(nameof(StackSample));

        Stack<SomeValue> stack = new();
        stack.Push(new SomeValue(1));
        stack.Push(new SomeValue(2));
        stack.Push(new SomeValue(3));
        Utilities.ShowMemoryAddress(ref stack);
        Console.WriteLine($"First: {stack.Peek()}");
        Console.WriteLine($"Pop: {stack.Pop()}");
        Console.WriteLine($"Pop: {stack.Pop()}");
        Console.WriteLine($"Pop: {stack.Pop()}");
        Console.WriteLine();
    }
}

