namespace Traits;

public class SnapshotList<T> : List<T>, IEnumerableEx<T>
{
    private readonly Dictionary<string, List<T>> _snapshots = [];
    private readonly Lock _snapshotLock = new();

    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
        // Create a simple string representation of the predicate for the snapshot
        var predicateKey = predicate.Method.ToString() ?? throw new InvalidOperationException("null returned from predicate.Method.ToString()");
        
        lock (_snapshotLock)
        {
            if (_snapshots.TryGetValue(predicateKey, out var snapshotList))
            {
                return snapshotList;
            }

            List<T> result = [];
            foreach (var item in this)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            
            _snapshots.Add(predicateKey, result);
            return result;
        }
    }
}
