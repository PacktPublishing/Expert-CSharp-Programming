using Books.Services;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Data.Common;

namespace Books.Data;

/// <summary>
/// Represents the database context for books.
/// </summary>
public class BooksContext(DbContextOptions<BooksContext> options, ILogger<BooksContext> logger) : DbContext(options), IBooksService
{
    /// <summary>
    /// Gets or sets the DbSet of books.
    /// </summary>
    public DbSet<Book> Books => Set<Book>();

    /// <summary>
    /// Retrieves all books asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of books.</returns>
    /// <exception cref="OperationCanceledException">Thrown if the operation is canceled.</exception>
    /// <exception cref="BookServiceException">Thrown if an error occurs while accessing the database.</exception>
    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var books = await Books.ToListAsync(cancellationToken);
            return books ?? [];
        }
        catch (SqliteException ex)
        {
            throw new BookServiceException(ex.Message, ex)
            {
                HResult = 3000
            };
        }
        catch (Exception ex) when (LogErrorFilter(ex))
        {
            throw;
        }
    }

    private bool LogErrorFilter(Exception ex)
    {
        logger.LogError(ex, "Error: {error}", ex.Message);
        return false;
    }   

    /// <summary>
    /// Retrieves a book by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the book to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The book with the specified ID, or null if not found.</returns>
    /// <exception cref="OperationCanceledException">Thrown if the operation is canceled.</exception>
    /// <exception cref="BookServiceException">Thrown if an error occurs while accessing the database.</exception>
    public async Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var books = await Books.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id, cancellationToken);
            return books;
        }
        catch (Exception ex) when (ex is InvalidOperationException or DbUpdateException)
        {
            throw new BookServiceException(ex.Message, ex);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Updates a book asynchronously.
    /// </summary>
    /// <param name="book">The book to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The number of affected rows.</returns>
    /// <exception cref="OperationCanceledException">Thrown if the operation is canceled.</exception>
    /// <exception cref="BookServiceException">Thrown if an error occurs while accessing the database.</exception>
    public async Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        try
        {
            int affected = await Books.Where(model => model.Id == book.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Title, book.Title)
                    .SetProperty(m => m.Publisher, book.Publisher),
                    cancellationToken);

            return affected;
        }
        catch (Exception ex) when (ex is DbUpdateException or DbException)
        {
            logger.LogError(ex, "Error: {error}", ex.Message);
            throw new BookServiceException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Creates a new book asynchronously.
    /// </summary>
    /// <param name="book">The book to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The created book.</returns>
    /// <exception cref="OperationCanceledException">Thrown if the operation is canceled.</exception>
    /// <exception cref="BookServiceException">An error is encountered while accessing the database.</exception>
    public async Task<Book> CreateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        try
        {
            Books.Add(book);
            await SaveChangesAsync(cancellationToken);
            return book;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error: {error}", ex.Message);
            throw new BookServiceException(ex.Message, ex);
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Error: {error}", ex.Message);
            throw new BookServiceException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Deletes a book by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The number of affected rows.</returns>
    /// <exception cref="OperationCanceledException">Thrown if the operation is canceled.</exception>
    /// <exception cref="BookServiceException">Thrown if an error occurs while accessing the database.</exception>
    public async Task<int> DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            int affected = await Books.Where(model => model.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return affected;
        }
        catch (Exception ex) when (ex is DbException or DbUpdateException)
        {
            logger.LogError(ex, "Error: {error}", ex.Message);
            throw new BookServiceException(ex.Message, ex);
        }
    }
}
