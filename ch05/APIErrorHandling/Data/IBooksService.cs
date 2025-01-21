using APIErrorHandling.Models;

namespace APIErrorHandling.Data;

public interface IBooksService
{
    Task<Book> CreateBookAsync(Book book);
    Task<int> DeleteBookAsync(int id);
    Task<Book?> GetBookByIdAsync(int id);
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<int> UpdateBookAsync(Book book);
}