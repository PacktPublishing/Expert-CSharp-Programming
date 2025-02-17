namespace Codebreaker.GameAPIs.Models;

public partial record class ShapeAndColorField(string Shape, string Color)
{
    internal static ShapeAndColorField Empty => new ShapeAndColorField(string.Empty, string.Empty);

    private const char Separator = ';';
    public override string ToString() => $"{Shape}{Separator}{Color}";

    public static implicit operator ShapeAndColorField((string Shape, string Color) f) => new(f.Shape, f.Color);
}
