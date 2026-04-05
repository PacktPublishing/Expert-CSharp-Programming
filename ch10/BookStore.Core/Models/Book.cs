namespace BookStore.Core.Models;

/// <summary>
/// Represents a book in the store catalog.
/// Uses a positional record for immutability and value-based equality.
/// </summary>
/// <param name="Id">Unique identifier for the book.</param>
/// <param name="Title">Title of the book.</param>
/// <param name="Author">Author's full name.</param>
/// <param name="Isbn">International Standard Book Number (ISBN-13).</param>
/// <param name="Price">Retail price in the store's default currency.</param>
/// <param name="Category">Genre or category of the book.</param>
/// <param name="StockQuantity">Number of copies currently in stock.</param>
/// <param name="PublishedDate">Date the book was first published.</param>
public record Book(
    int Id,
    string Title,
    string Author,
    string Isbn,
    decimal Price,
    BookCategory Category,
    int StockQuantity,
    DateOnly PublishedDate);
