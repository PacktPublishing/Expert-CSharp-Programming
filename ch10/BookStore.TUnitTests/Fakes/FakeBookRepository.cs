using BookStore.Core.Models;
using BookStore.Core.Repositories;

namespace BookStore.TUnitTests.Fakes;

/// <summary>
/// A compile-time test double for <see cref="IBookRepository"/>.
/// Uses an in-memory dictionary to provide realistic state-based behaviour
/// without any runtime IL-emit or proxy generation.
///
/// The partial modifier leaves the type open for future source-generator
/// extensions (e.g. call-recording infrastructure).
/// </summary>
public sealed partial class FakeBookRepository(IEnumerable<Book>? seed = null)
    : IBookRepository
{
    private readonly Dictionary<int, Book> _store =
        seed?.ToDictionary(b => b.Id) ?? [];

    // Start the ID counter just above the highest seeded ID so AddAsync never
    // assigns a duplicate.  seed?.MaxBy(...)?.Id is null when seed is null or
    // empty, so the ?? 1 fallback ensures _nextId starts at 1.
    private int _nextId = seed?.MaxBy(b => b.Id)?.Id + 1 ?? 1;

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => Task.FromResult(_store.GetValueOrDefault(id));

    public Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<Book>>([.. _store.Values]);

    public Task<IReadOnlyList<Book>> GetByCategoryAsync(
        BookCategory category,
        CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<Book>>(
            [.. _store.Values.Where(b => b.Category == category)]);

    public Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        var saved = book with { Id = _nextId++ };
        _store[saved.Id] = saved;
        return Task.FromResult(saved);
    }

    public Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        _store[book.Id] = book;
        return Task.FromResult(book);
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _store.Remove(id);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => Task.FromResult(_store.ContainsKey(id));
}
