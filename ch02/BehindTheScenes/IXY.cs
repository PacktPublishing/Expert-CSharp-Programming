
public interface IXY
{
    int X { get; init; }
    int Y { get; init; }
    void Deconstruct(out int x, out int y);
}
