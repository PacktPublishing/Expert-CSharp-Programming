using System.Data.Common;

using Books.Data.Extensions;
using Books.Services;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        logger.GetBooksStart();
        try
        {
            var books = await Books.ToListAsync(cancellationToken);
            var list = books ?? [];
            logger.GetBooksSuccess(list.Count);
            return list;
        }
        catch (SqliteException ex)
        {
            logger.GetBooksError(ex, ex.Message);
            throw new BookServiceException(ex.Message, ex) { HResult = 3000 };
        }
        catch (Exception ex) when (LogErrorFilter(ex))
        {
            throw;
        }
    }

    private bool LogErrorFilter(Exception ex)
    {
        try
        {
            if (ex is OperationCanceledException)
            {
                return false;
            }
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.GetBooksError(ex, ex.Message);
            }
        }
        catch { }
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
        logger.GetBookByIdStart(id);
        try
        {
            var book = await Books.AsNoTracking().FirstOrDefaultAsync(model => model.Id == id, cancellationToken);
            return book;
        }
        catch (Exception ex) when (ex is InvalidOperationException or DbUpdateException)
        {
            logger.GetBookByIdError(ex, id, ex.Message);
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
        logger.UpdateBookStart(book.Id);
        try
        {
            int affected = await Books.Where(model => model.Id == book.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Title, book.Title)
                    .SetProperty(m => m.Publisher, book.Publisher),
                    cancellationToken);
            logger.UpdateBookSuccess(book.Id, affected);
            return affected;
        }
        catch (Exception ex) when (ex is DbUpdateException or DbException)
        {
            logger.UpdateBookError(ex, book.Id, ex.Message);
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
        logger.CreateBookStart(book.Title);
        try
        {
            Books.Add(book);
            await SaveChangesAsync(cancellationToken);
            logger.CreateBookSuccess(book.Title, book.Id);
            return book;
        }
        catch (DbUpdateException ex)
        {
            logger.CreateBookError(ex, ex.Message);
            throw new BookServiceException(ex.Message, ex);
        }
        catch (DbException ex)
        {
            logger.CreateBookError(ex, ex.Message);
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
        logger.DeleteBookStart(id);
        try
        {
            int affected = await Books.Where(model => model.Id == id).ExecuteDeleteAsync(cancellationToken);
            logger.DeleteBookSuccess(id, affected);
            return affected;
        }
        catch (Exception ex) when (ex is DbException or DbUpdateException)
        {
            logger.DeleteBookError(ex, id, ex.Message);
            throw new BookServiceException(ex.Message, ex);
        }
    }
}
