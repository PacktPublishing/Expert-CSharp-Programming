public class AVLTree
{
    /// <summary>
    /// Represents a node in the AVL tree.
    /// </summary>
    private class Node(int value)
    {
        /// <summary>
        /// The value stored in the node.
        /// </summary>
        public int Value = value;

        /// <summary>
        /// The left child of the node.
        /// </summary>
        public Node? Left;

        /// <summary>
        /// The right child of the node.
        /// </summary>
        public Node? Right;

        /// <summary>
        /// The height of the node in the tree.
        /// </summary>
        public int Height = 1;
    }

    /// <summary>
    /// The root node of the AVL tree.
    /// </summary>
    private Node? root;

    /// <summary>
    /// Inserts a value into the AVL tree.
    /// </summary>
    /// <param name="value">The value to insert.</param>
    public void Insert(int value)
    {
        root = Insert(root, value);
    }

    /// <summary>
    /// Recursively inserts a value into the tree and balances it if necessary.
    /// </summary>
    /// <param name="node">The current node being processed.</param>
    /// <param name="value">The value to insert.</param>
    /// <returns>The updated node after insertion and balancing.</returns>
    private Node Insert(Node? node, int value)
    {
        if (node is null)
        {
            return new Node(value);
        }

        if (value < node.Value)
        {
            node.Left = Insert(node.Left, value);
        }
        else if (value > node.Value)
        {
            node.Right = Insert(node.Right, value);
        }
        else
        {
            // Duplicate values are not allowed in this AVL tree
            return node;
        }

        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));

        int balance = GetBalance(node);

        // Left Left Case
        if (balance > 1 && value < node.Left?.Value)
        {
            return RightRotate(node);
        }

        // Right Right Case
        if (balance < -1 && value > node.Right?.Value)
        {
            return LeftRotate(node);
        }

        // Left Right Case
        if (balance > 1 && value > node.Left.Value)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        // Right Left Case
        if (balance < -1 && value < node.Right.Value)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }

    /// <summary>
    /// Gets the height of a node.
    /// </summary>
    /// <param name="node">The node to check.</param>
    /// <returns>The height of the node, or 0 if the node is null.</returns>
    private int Height(Node? node)
    {
        return node?.Height ?? 0;
    }

    /// <summary>
    /// Calculates the balance factor of a node.
    /// </summary>
    /// <param name="node">The node to calculate the balance for.</param>
    /// <returns>The balance factor of the node.</returns>
    private int GetBalance(Node node)
    {
        return Height(node.Left) - Height(node.Right);
    }

    /// <summary>
    /// Performs a right rotation on the given node.
    /// </summary>
    /// <param name="y">The node to rotate.</param>
    /// <returns>The new root of the subtree after rotation.</returns>
    private Node RightRotate(Node y)
    {
        Node x = y.Left;
        Node T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

        return x;
    }

    /// <summary>
    /// Performs a left rotation on the given node.
    /// </summary>
    /// <param name="x">The node to rotate.</param>
    /// <returns>The new root of the subtree after rotation.</returns>
    private Node LeftRotate(Node x)
    {
        Node? y = x.Right;
        Node? T2 = y?.Left;

        y?.Left = x;
        x.Right = T2;

        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
        y?.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

        return y;
    }

    /// <summary>
    /// Searches for a value in the AVL tree.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>True if the value is found, otherwise false.</returns>
    public bool Search(int value)
    {
        return Search(root, value);
    }

    /// <summary>
    /// Recursively searches for a value in the tree.
    /// </summary>
    /// <param name="node">The current node being processed.</param>
    /// <param name="value">The value to search for.</param>
    /// <returns>True if the value is found, otherwise false.</returns>
    private bool Search(Node? node, int value)
    {
        if (node is null)
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
