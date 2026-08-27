namespace ClassesSample;

public partial class Film
{
    /// <summary>
    /// Creates a copy of this Film instance
    /// </summary>
    /// <returns>A new Film instance with the same property values</returns>
    public virtual Film Clone()
    {
        return new Film(Title, Director, Year);
    }

    /// <summary>
    /// Creates a copy of this Film instance with optionally modified properties
    /// </summary>
    /// <param name="title">New title value, or null to keep current value</param>
    /// <param name="director">New director value, or null to keep current value</param>
    /// <param name="year">New year value, or null to keep current value</param>
    /// <returns>A new Film instance with the specified modifications</returns>
    public virtual Film Clone(string? title = null, string? director = null, int? year = null)
    {
        return new Film(
            title ?? Title,
            director ?? Director,
            year ?? Year
        );
    }
}

// Extension for ExtendedFilm to support Genre cloning
public partial class ExtendedFilm
{
    /// <summary>
    /// Creates a copy of this ExtendedFilm instance
    /// </summary>
    /// <returns>A new ExtendedFilm instance with the same property values</returns>
    public override Film Clone()
    {
        return new ExtendedFilm(Title, Director, Genre, Year);
    }

    /// <summary>
    /// Creates a copy of this ExtendedFilm instance with optionally modified properties
    /// </summary>
    /// <param name="title">New title value, or null to keep current value</param>
    /// <param name="director">New director value, or null to keep current value</param>
    /// <param name="year">New year value, or null to keep current value</param>
    /// <param name="genre">New genre value, or null to keep current value</param>
    /// <returns>A new ExtendedFilm instance with the specified modifications</returns>
    public ExtendedFilm Clone(string? title = null, string? director = null, int? year = null, string? genre = null)
    {
        return new ExtendedFilm(
            title ?? Title,
            director ?? Director,
            genre ?? Genre,
            year ?? Year
        );
    }
}
