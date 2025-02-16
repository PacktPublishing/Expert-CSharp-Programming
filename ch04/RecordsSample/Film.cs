using System.Text;

namespace RecordsSample;

public record class Film(string Title, string? Director = default, int Year = default);

public record class ExtendedFilm(string Title, string? Director, string? Genre, int Year) : Film(Title, Director, Year)
{
    //protected override Type EqualityContract => base.EqualityContract;

    //public override string ToString() 
    //{
    //    StringBuilder builder = new();
    //    if (PrintMembers(builder))
    //    {
    //        return builder.ToString();
    //    }
    //    else
    //    {
    //        return base.ToString();
    //    }
    //}

    protected override bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Title: ");
        builder.Append(Title);
        builder.Append(", ");
        builder.Append("Director: ");
        builder.Append(Director);
        builder.Append(", ");
        builder.Append("Genre: ");
        builder.Append(Genre);
        builder.Append(", ");
        builder.Append("Year: ");
        builder.Append(Year);
        return false;
    }
}
