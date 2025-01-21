namespace APIErrorHandling.Models;

public class Book(string title, int id = 0, string? publisher = default)
{
    public int Id { get; init; } = id;
    public string Title { get; set; } = title;
    public string? Publisher { get; set;} = publisher;
}
