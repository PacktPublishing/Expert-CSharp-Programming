namespace ClassesSample;

public partial class Film
{
    public void Deconstruct(out string title, out string? director, out int year)
    {
        title = Title;
        director = Director;
        year = Year;
    }
}
