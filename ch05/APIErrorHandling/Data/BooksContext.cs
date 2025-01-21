using APIErrorHandling.Models;

using Microsoft.EntityFrameworkCore;

namespace APIErrorHandling.Data;

public class BooksContext(DbContextOptions<BooksContext> options) : DbContext(options), IBooksService
{
    public DbSet<Book> Books => Set<Book>();

    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        var books = await Books.ToListAsync();
        return books ?? Enumerable.Empty<Book>();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await Books.AsNoTracking()
            .FirstOrDefaultAsync(model => model.Id == id);
    }

    public async Task<int> UpdateBookAsync(Book book)
    {
        int affected = await Books.Where(model => model.Id == book.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.Title, book.Title)
                .SetProperty(m => m.Publisher, book.Publisher)
                );

        return affected;
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        Books.Add(book);
        await SaveChangesAsync();
        return book;
    }

    public async Task<int> DeleteBookAsync(int id)
    {
        int affected = await Books.Where(model => model.Id == id)
            .ExecuteDeleteAsync();
        return affected;
    }
}
