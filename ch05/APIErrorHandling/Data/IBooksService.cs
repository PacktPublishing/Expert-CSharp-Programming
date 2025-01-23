using APIErrorHandling.Models;

namespace APIErrorHandling.Data;

public interface IBooksService
{
    Task<Book> CreateBookAsync(Book book, CancellationToken cancellationToken = default);
    Task<int> DeleteBookAsync(int id, CancellationToken cancellationToken = default);
    Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellation = default);
    Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken = default);
}