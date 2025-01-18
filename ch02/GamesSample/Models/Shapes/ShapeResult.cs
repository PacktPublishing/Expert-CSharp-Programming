namespace GamesSample.Models;

#if USERECORDS
public readonly partial record struct ShapeResult(int CorrectPosition, int IncorrectPosition, int PartialCorrect)
{
    public const string Separator = ":";
    public override string ToString() => $"{CorrectPosition}{Separator}{IncorrectPosition}{Separator}{PartialCorrect}";
}
#endif