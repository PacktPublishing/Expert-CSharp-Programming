using System.Collections.Concurrent;
using BookStore.Core.Models;
using BookStore.Core.Repositories;

namespace BookStore.Api.Repositories;

/// <summary>
/// Thread-safe in-memory implementation of <see cref="IBookRepository"/>.
/// Pre-seeded with sample data for demonstration purposes.
/// </summary>
public sealed class InMemoryBookRepository : IBookRepository
{
    private readonly ConcurrentDictionary<int, Book> _books;
    private int _nextId;

    public InMemoryBookRepository()
    {
        Book[] seed =
        [
            new(1, "The Pragmatic Programmer", "David Thomas", "978-0135957059", 49.99m, BookCategory.Technology, 15, new DateOnly(2019, 9, 13)),
            new(2, "Clean Code", "Robert C. Martin", "978-0132350884", 44.99m, BookCategory.Technology, 8, new DateOnly(2008, 8, 1)),
            new(3, "Domain-Driven Design", "Eric Evans", "978-0321125217", 59.99m, BookCategory.Technology, 5, new DateOnly(2003, 8, 30)),
            new(4, "A Brief History of Time", "Stephen Hawking", "978-0553380163", 14.99m, BookCategory.Science, 20, new DateOnly(1998, 9, 1)),
            new(5, "Sapiens", "Yuval Noah Harari", "978-0062316097", 17.99m, BookCategory.History, 12, new DateOnly(2015, 2, 10)),
        ];

        _books = new ConcurrentDictionary<int, Book>(seed.ToDictionary(b => b.Id));
        _nextId = seed.Max(b => b.Id); // Interlocked.Increment returns _nextId+1, so first new ID = max+1
    }

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_books.TryGetValue(id, out var book) ? book : null);

    public Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Book>>([.. _books.Values.OrderBy(b => b.Id)]);

    public Task<IReadOnlyList<Book>> GetByCategoryAsync(BookCategory category, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Book>>([.. _books.Values.Where(b => b.Category == category).OrderBy(b => b.Id)]);

    public Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        var id = Interlocked.Increment(ref _nextId);
        var saved = book with { Id = id };
        _books[id] = saved;
        return Task.FromResult(saved);
    }

    public Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        _books[book.Id] = book;
        return Task.FromResult(book);
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _books.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_books.ContainsKey(id));
}
