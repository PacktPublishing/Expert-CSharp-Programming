/// <summary>
/// Represents a Bloom Filter, a probabilistic data structure that is used to test whether an element is a member of a set.
/// </summary>
public class BloomFilter
{
    private readonly bool[] _bits;
    private readonly int _size;

    /// <summary>
    /// Initializes a new instance of the <see cref="BloomFilter"/> class with the specified size.
    /// </summary>
    /// <param name="size">The size of the Bloom Filter.</param>
    public BloomFilter(int size)
    {
        _bits = new bool[size];
        _size = size;
    }

    /// <summary>
    /// Adds an item to the Bloom Filter.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void Add(int item)
    {
        int hash1 = item.GetHashCode() % _size;
        int hash2 = (item.GetHashCode() / _size) % _size;
        _bits[hash1] = true;
        _bits[hash2] = true;
    }

    /// <summary>
    /// Checks if the Bloom Filter contains the specified item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns><c>true</c> if the item is probably in the set; otherwise, <c>false</c>.</returns>
    public bool Contains(int item)
    {
        int hash1 = item.GetHashCode() % _size;
        int hash2 = (item.GetHashCode() / _size) % _size;
        return _bits[hash1] && _bits[hash2];
    }
}
