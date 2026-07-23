namespace Synchronization;

/// <summary>Thread-safe counter using lock for mutual exclusion.</summary>
internal sealed class Counter
{
    private readonly Lock _lock = new();
    private int _value;

    public void Increment() 
    { 
        lock (_lock) 
        { 
            _value++; 
        } 
    }
    public int Value 
    { 
        get 
        { 
            lock (_lock) 
            { 
                return _value; 
            } 
        } 
    }
}