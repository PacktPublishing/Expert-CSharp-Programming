using BookStore.Core.Models;

namespace BookStore.Core.Repositories;

/// <summary>
/// Defines data-access operations for books.
/// Implementations may use an in-memory store, database, or any other persistence layer.
/// </summary>
public interface IBookRepository
{
    /// <summary>Returns the book with the given <paramref name="id"/>, or <see langword="null"/>.</summary>
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Returns all books in the store.</summary>
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns all books belonging to <paramref name="category"/>.</summary>
    Task<IReadOnlyList<Book>> GetByCategoryAsync(BookCategory category, CancellationToken cancellationToken = default);

    /// <summary>Persists a new book and returns the saved entity (with assigned ID).</summary>
    Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing book and returns the updated entity.</summary>
    Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken = default);

    /// <summary>Removes the book with the given <paramref name="id"/>.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Returns <see langword="true"/> if a book with the given <paramref name="id"/> exists.</summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
