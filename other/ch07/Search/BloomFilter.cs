/// <summary>
/// Represents a Bloom Filter, a probabilistic data structure that is used to test whether an element is a member of a set.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BloomFilter"/> class with the specified size.
/// </remarks>
/// <param name="size">The size of the Bloom Filter.</param>
public class BloomFilter(int size)
{
    private readonly bool[] _bits = new bool[size];

    /// <summary>
    /// Adds an item to the Bloom Filter.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void Add(int item)
    {
        int hash1 = item.GetHashCode() % size;
        int hash2 = (item.GetHashCode() / size) % size;
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
        int hash1 = item.GetHashCode() % size;
        int hash2 = (item.GetHashCode() / size) % size;
        return _bits[hash1] && _bits[hash2];
    }
}
