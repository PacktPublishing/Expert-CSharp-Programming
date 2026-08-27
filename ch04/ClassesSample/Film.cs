using System.Text;

namespace ClassesSample;

public partial class Film(string title, string? director = default, int year = default)
{
    public string Title { get; init; } = title;
    public string? Director { get; init; } = director;
    public int Year { get; init; } = year;

    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Title = ");
        builder.Append(Title);
        builder.Append(", Director = ");
        builder.Append(Director);       
        builder.Append(", Year = ");
        builder.Append(Year.ToString());        
        return true;
    }

    public override string ToString()
    {
        StringBuilder builder = new();
        builder.Append("Film { ");
        if (PrintMembers(builder))
        {
            builder.Append(" }");
            return builder.ToString();
        }
        else
        {
            return base.ToString() ?? string.Empty;
        }
    }
}

public partial class ExtendedFilm(string title, string? director, string? genre, int year) : Film(title, director, year)
{
    public string? Genre { get; init; } = genre;

    protected override Type EqualityContract => typeof(ExtendedFilm);

    protected override bool PrintMembers(StringBuilder builder)
    {
        // Call base implementation first
        bool hasMembers = base.PrintMembers(builder);
        
        if (hasMembers)
        {
            builder.Append(", ");
        }
        
        builder.Append("Genre = ");
        builder.Append((object?)Genre);
        
        return true;
    }
}
