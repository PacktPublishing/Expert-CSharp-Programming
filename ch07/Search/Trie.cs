/// <summary>
/// Represents a Trie (prefix tree) data structure for storing strings.
/// </summary>
public class Trie
{
    /// <summary>
    /// Represents a node in the Trie.
    /// </summary>
    private class TrieNode
    {
        /// <summary>
        /// Gets or sets the children of the current node.
        /// </summary>
        public Dictionary<char, TrieNode> Children = [];

        /// <summary>
        /// Gets or sets a value indicating whether the current node is the end of a word.
        /// </summary>
        public bool IsEndOfWord { get; set; }
    }

    /// <summary>
    /// The root node of the Trie.
    /// </summary>
    private readonly TrieNode root = new();

    /// <summary>
    /// Inserts a word into the Trie.
    /// </summary>
    /// <param name="word">The word to insert.</param>
    public void Insert(string word)
    {
        var node = root;
        foreach (var ch in word)
        {
            if (!node.Children.ContainsKey(ch))
            {
                node.Children[ch] = new TrieNode();
            }
            node = node.Children[ch];
        }
        node.IsEndOfWord = true;
    }

    /// <summary>
    /// Searches for a word in the Trie.
    /// </summary>
    /// <param name="word">The word to search for.</param>
    /// <returns>True if the word is found; otherwise, false.</returns>
    public bool Search(string word)
    {
        var node = root;
        foreach (var ch in word)
        {
            if (!node.Children.TryGetValue(ch, out TrieNode? value))
            {
                return false;
            }
            node = value;
        }
        return node.IsEndOfWord;
    }
}
