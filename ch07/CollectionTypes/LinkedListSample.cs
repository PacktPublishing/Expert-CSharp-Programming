
using System.Runtime.InteropServices;

namespace CollectionTypes;

public class LinkedListSample
{
    public static void Run()
    {
        LinkedList<SomeValue> linkedList = new();
        linkedList.AddFirst(new SomeValue(1, 1));
        linkedList.AddLast(new SomeValue(2, 2));
        linkedList.AddLast(new SomeValue(3, 3));
        for (var node = linkedList.First; node != null; node = node.Next)
        {
            Utilities.ShowMemoryAddress(ref node);  // display the address of the LinkedListNode
            Utilities.ShowMemoryAddress(ref node.ValueRef);  // display the address of the SomeValue
            Utilities.ShowMemoryAddress1(ref node.ValueRef);  // display the address of the SomeValue
        }

        Console.WriteLine($"First: {linkedList.First?.Value}");
        Console.WriteLine($"Last: {linkedList.Last?.Value}");
        linkedList.RemoveFirst();
        linkedList.RemoveLast();
        for (var node = linkedList.First; node != null; node = node.Next)
        {
            Utilities.ShowMemoryAddress(ref node);
            Utilities.ShowMemoryAddress(ref node.ValueRef);
        } 
    }
}

public record class SomeData(int X, int Y);
public record struct SomeValue(int X, int Y);

public static class Utilities
{
    //public static void ShowMemoryAddress<T>(ref readonly T item)
    //{
    //    GCHandle handle = GCHandle.Alloc(item, GCHandleType.Pinned);
    //    nint address = handle.AddrOfPinnedObject();
    //    handle.Free();
    //    Console.WriteLine($"{item} with address {address:X}");
    //}

    public static unsafe void ShowMemoryAddress<T>(ref T item)
    {
        TypedReference tr = __makeref(item);
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
        IntPtr ptr = **(IntPtr**)&tr;
#pragma warning restore CS8500 // We just use the address to display the address, we don't access the value, thus pinning is not necessary
        Console.WriteLine($"{item} with address {ptr:X}");
    }

    public static unsafe void ShowMemoryAddress1<T>(ref T item)
    {
        fixed (T* p = &item)
        {
            Console.WriteLine($"{item} with address {(nint)p:X}");
        }
//        TypedReference tr = __makeref(item);
//#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
//        IntPtr ptr = **(IntPtr**)&tr;
//#pragma warning restore CS8500 // We just use the address to display the address, we don't access the value, thus pinning is not necessary
//        Console.WriteLine($"{item} with address {ptr:X}");
    }
}