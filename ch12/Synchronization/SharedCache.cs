namespace Synchronization;

// <summary>Read-heavy cache protected by ReaderWriterLockSlim.</summary>
internal sealed class SharedCache : IDisposable
{
    private readonly ReaderWriterLockSlim _rwLock = new();
    private string _data = "initial-value";

    public string Read()
    {
        _rwLock.EnterReadLock();
        try 
        { 
            return _data; 
        }
        finally 
        { 
            _rwLock.ExitReadLock(); 
        }
    }

    public void Write(string value)
    {
        _rwLock.EnterWriteLock();
        try 
        { 
            _data = value; 
        }
        finally 
        { 
            _rwLock.ExitWriteLock(); 
        }
    }

    public void Dispose() => _rwLock.Dispose();
}
