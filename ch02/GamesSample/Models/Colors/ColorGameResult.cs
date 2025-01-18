namespace GamesSample.Models;

#if USERECORDS
public readonly partial record struct ColorGameResult(int CorrectPosition, int IncorrectPosition)
{
    public const string Separator = ":";
    public override string ToString() => $"{CorrectPosition}{Separator}{IncorrectPosition}";
}
#endif