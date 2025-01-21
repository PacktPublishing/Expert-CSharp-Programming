using APIErrorHandling.Models;

using Microsoft.EntityFrameworkCore;

namespace APIErrorHandling.Data;

public class BooksContext : DbContext
{
    public BooksContext (DbContextOptions<BooksContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Book { get; set; } = default!;
}
