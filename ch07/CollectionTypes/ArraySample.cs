using System.Runtime.InteropServices;

namespace CollectionTypes;
internal class ArraySample : IShowTitle
{
    public static void Run()
    {
        IShowTitle.ShowTitle(nameof(ArraySample));

        int[] array1 = [1, 2, 3, 4];

        ShowArrayElementsAddresses("int array", array1);
        Array.Reverse(array1);
        ShowArrayElementsAddresses("Reversed int array", array1);

        SomeValue[] array2 = [new SomeValue(1), new SomeValue(2), new SomeValue(3), new SomeValue(4)];
        ShowArrayElementsAddresses("Array with value types", array2);
        Array.Reverse(array2);
        ShowArrayElementsAddresses("Reversed SomeValue array", array2);

        SomeData[] array3 = [new SomeData(1), new SomeData(2), new SomeData(3), new SomeData(4)];
        ShowArrayElementsAddresses("Array with reference types", array3);
        ShowHeapAddresses("Heap addresses of SomeData array", array3);
        Array.Reverse(array3);
        ShowArrayElementsAddresses("Reversed SomeData array", array3);
        ShowHeapAddresses("Heap addresses of reversed SomeData array", array3);
    }

    public unsafe static void ShowArrayElementsAddresses<T>(
        string message, T[] array)
    {
        Console.WriteLine(message);
        for (int i = 0; i < array.Length; i++)
        {
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
            fixed (T* p = &array[i])
            {
                Console.WriteLine(
                  $"Element {i} in the array: {(nint)p:X} with value {array[i]}");
            }
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
        }
        Console.WriteLine();
    }

    public static void ShowHeapAddresses<T>(string message, T[] array) where T : class
    {
        Console.WriteLine(message);
        for (int i = 0; i < array.Length; i++)
        {
            // Pin the object and get its address
            var handle = GCHandle.Alloc(array[i], GCHandleType.Pinned);
            try
            {
                IntPtr address = handle.AddrOfPinnedObject();
                Console.WriteLine($"Element {i} in the array: {address:X} with value {array[i]}");
            }
            finally
            {
                handle.Free(); // Always free the handle to avoid memory leaks
            }
        }
        Console.WriteLine();
    }
}
