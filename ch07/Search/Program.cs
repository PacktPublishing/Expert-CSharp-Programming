List<int> list = [1, 3, 5, 7, 9, 11, 13, 15, 17, 19];
list.BinarySearch(7);

// Linear Search
int target = 7;
int linearSearchResult = LinearSearch(list, target);
Console.WriteLine($"Linear Search: Found {target} at index {linearSearchResult}");

// Binary Search
int binarySearchResult = BinarySearch(list, target);
Console.WriteLine($"Binary Search: Found {target} at index {binarySearchResult}");

// Hash-based Search
HashSet<int> hashSet = new(list);
bool hashSearchResult = hashSet.Contains(target);
Console.WriteLine($"Hash-based Search: Found {target} = {hashSearchResult}");

// Binary Search Tree
BinarySearchTree bst = new();
foreach (var item in list)
{
    bst.Insert(item);
}
bool bstSearchResult = bst.Search(target);
Console.WriteLine($"Binary Search Tree: Found {target} = {bstSearchResult}");

// Trie
Trie trie = new();
foreach (var item in list.Select(i => i.ToString()))
{
    trie.Insert(item);
}
bool trieSearchResult = trie.Search(target.ToString());
Console.WriteLine($"Trie: Found {target} = {trieSearchResult}");

// Bloom Filter
BloomFilter bloomFilter = new(100);
foreach (var item in list)
{
    bloomFilter.Add(item);
}
bool bloomFilterSearchResult = bloomFilter.Contains(target);
Console.WriteLine($"Bloom Filter: Found {target} = {bloomFilterSearchResult}");


static int LinearSearch(List<int> list, int target)
{
    for (int i = 0; i < list.Count; i++)
    {
        if (list[i] == target)
        {
            return i;
        }
    }
    return -1;
}

static int BinarySearch(List<int> list, int target)
{
    int left = 0;
    int right = list.Count - 1;
    while (left <= right)
    {
        int mid = left + (right - left) / 2;
        if (list[mid] == target)
        {
            return mid;
        }
        if (list[mid] < target)
        {
            left = mid + 1;
        }
        else
        {
            right = mid - 1;
        }
    }
    return -1;
}


// Binary Search Tree
public class BinarySearchTree
{
    private class Node
    {
        public int Value;
        public Node? Left;
        public Node? Right;

        public Node(int value)
        {
            Value = value;
        }
    }

    private Node? root;

    public void Insert(int value)
    {
        root = Insert(root, value);
    }

    private Node Insert(Node? node, int value)
    {
        if (node == null)
        {
            return new Node(value);
        }
        if (value < node.Value)
        {
            node.Left = Insert(node.Left, value);
        }
        else
        {
            node.Right = Insert(node.Right, value);
        }
        return node;
    }

    public bool Search(int value)
    {
        return Search(root, value);
    }

    private bool Search(Node? node, int value)
    {
        if (node == null)
        {
            return false;
        }
        if (node.Value == value)
        {
            return true;
        }
        if (value < node.Value)
        {
            return Search(node.Left, value);
        }
        else
        {
            return Search(node.Right, value);
        }
    }
}
