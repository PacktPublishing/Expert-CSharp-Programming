namespace JsonSerialization.Models;

/// <summary>A library that owns a collection of books.</summary>
public record class Library(
    string Name,
    string Location,
    IReadOnlyList<Book> Books)
{
    public int TotalBooks => Books.Count;
}
