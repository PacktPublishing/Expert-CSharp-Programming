using BookStore.Core.Models;

namespace BookStore.Core.Services;

/// <summary>
/// Provides business-level operations over the book catalog.
/// </summary>
public interface IBookService
{
    /// <summary>Returns the book with the given <paramref name="id"/>, or <see langword="null"/>.</summary>
    Task<Book?> GetBookAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Returns all books available in the store.</summary>
    Task<IReadOnlyList<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns books filtered by <paramref name="category"/>.</summary>
    Task<IReadOnlyList<Book>> GetBooksByCategoryAsync(BookCategory category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new book to the catalog after validation.
    /// </summary>
    /// <exception cref="ArgumentNullException">When <paramref name="book"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">When title is empty or price is negative.</exception>
    Task<Book> AddBookAsync(Book book, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <exception cref="BookStore.Core.Exceptions.BookNotFoundException">When the book does not exist.</exception>
    Task<Book> UpdateBookAsync(Book book, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a book from the catalog.
    /// </summary>
    /// <exception cref="BookStore.Core.Exceptions.BookNotFoundException">When the book does not exist.</exception>
    Task RemoveBookAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates the discounted price for a book.
    /// </summary>
    /// <param name="bookId">The ID of the book.</param>
    /// <param name="discountPercentage">A value between 0 and 100.</param>
    /// <returns>The price after applying the discount.</returns>
    /// <exception cref="ArgumentOutOfRangeException">When <paramref name="discountPercentage"/> is outside [0, 100].</exception>
    /// <exception cref="BookStore.Core.Exceptions.BookNotFoundException">When the book does not exist.</exception>
    Task<decimal> CalculateDiscountPriceAsync(int bookId, decimal discountPercentage, CancellationToken cancellationToken = default);

    /// <summary>Returns <see langword="true"/> when the book has at least one copy in stock.</summary>
    /// <exception cref="BookStore.Core.Exceptions.BookNotFoundException">When the book does not exist.</exception>
    Task<bool> IsInStockAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for books whose title, author, or ISBN contains <paramref name="searchTerm"/>.
    /// Returns all books when <paramref name="searchTerm"/> is null or whitespace.
    /// </summary>
    Task<IReadOnlyList<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken = default);
}
