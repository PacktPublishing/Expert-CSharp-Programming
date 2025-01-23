using APIErrorHandling.Models;

using Microsoft.EntityFrameworkCore;

namespace APIErrorHandling.Data;

public class BooksContext(DbContextOptions<BooksContext> options) : DbContext(options), IBooksService
{
    public DbSet<Book> Books => Set<Book>();

    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken)
    {
        var books = await Books.ToListAsync(cancellationToken);
        return books ?? Enumerable.Empty<Book>();
    }

    public async Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Books.AsNoTracking()
            .FirstOrDefaultAsync(model => model.Id == id, cancellationToken);
    }

    public async Task<int> UpdateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        int affected = await Books.Where(model => model.Id == book.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.Title, book.Title)
                .SetProperty(m => m.Publisher, book.Publisher), 
                cancellationToken);

        return affected;
    }

    public async Task<Book> CreateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        Books.Add(book);
        await SaveChangesAsync(cancellationToken);
        return book;
    }

    public async Task<int> DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        int affected = await Books.Where(model => model.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        return affected;
    }
}
