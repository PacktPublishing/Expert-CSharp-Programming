public class AVLTree
{
    private class Node
    {
        public int Value;
        public Node? Left;
        public Node? Right;
        public int Height;

        public Node(int value)
        {
            Value = value;
            Height = 1;
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
        if (balance > 1 && value < node.Left.Value)
        {
            return RightRotate(node);
        }

        // Right Right Case
        if (balance < -1 && value > node.Right.Value)
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

    private int Height(Node? node)
    {
        return node?.Height ?? 0;
    }

    private int GetBalance(Node node)
    {
        return Height(node.Left) - Height(node.Right);
    }

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

    private Node LeftRotate(Node x)
    {
        Node y = x.Right;
        Node T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

        return y;
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
