namespace CollectionTypes;

public static class Utilities
{
    public static unsafe void ShowMemoryAddressInd<T>(ref T item)
    {
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
        fixed (T* p = &item)
        {
            Console.WriteLine($"{item} with address {(nint)p:X}");
        }
#pragma warning restore CS8500
    }
    public static unsafe void ShowMemoryAddress<T>(ref T item)
    {
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

        fixed (T* p = &item)
        {
            Console.WriteLine($"{item?.GetType().Name} - {item} with address {(nint)p:X}");
        }
#pragma warning restore CS8500
    }
}

public interface IShowTitle
{
    static void ShowTitle(string className)
    {
        string title = new([.. className[..^6], ' ', .. className[^6..]]);
        Console.WriteLine(title);
    }
}