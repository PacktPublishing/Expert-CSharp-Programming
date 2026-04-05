using BookStore.Core.Exceptions;
using BookStore.Core.Models;
using BookStore.Core.Repositories;

namespace BookStore.Core.Services;

/// <summary>
/// Default implementation of <see cref="IBookService"/> that delegates
/// persistence to an <see cref="IBookRepository"/>.
/// </summary>
public sealed class BookService(IBookRepository repository) : IBookService
{
    public Task<Book?> GetBookAsync(int id, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyList<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default) =>
        repository.GetAllAsync(cancellationToken);

    public Task<IReadOnlyList<Book>> GetBooksByCategoryAsync(BookCategory category, CancellationToken cancellationToken = default) =>
        repository.GetByCategoryAsync(category, cancellationToken);

    public async Task<Book> AddBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Book title cannot be empty.", nameof(book));

        if (book.Price < 0)
            throw new ArgumentException("Book price cannot be negative.", nameof(book));

        return await repository.AddAsync(book, cancellationToken);
    }

    public async Task<Book> UpdateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Book title cannot be empty.", nameof(book));

        if (book.Price < 0)
            throw new ArgumentException("Book price cannot be negative.", nameof(book));

        if (!await repository.ExistsAsync(book.Id, cancellationToken))
            throw new BookNotFoundException(book.Id);

        return await repository.UpdateAsync(book, cancellationToken);
    }

    public async Task RemoveBookAsync(int id, CancellationToken cancellationToken = default)
    {
        if (!await repository.ExistsAsync(id, cancellationToken))
            throw new BookNotFoundException(id);

        await repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<decimal> CalculateDiscountPriceAsync(
        int bookId,
        decimal discountPercentage,
        CancellationToken cancellationToken = default)
    {
        if (discountPercentage is < 0 or > 100)
            throw new ArgumentOutOfRangeException(
                nameof(discountPercentage),
                "Discount percentage must be between 0 and 100.");

        var book = await repository.GetByIdAsync(bookId, cancellationToken)
            ?? throw new BookNotFoundException(bookId);

        return book.Price * (1 - discountPercentage / 100);
    }

    public async Task<bool> IsInStockAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new BookNotFoundException(id);

        return book.StockQuantity > 0;
    }

    public async Task<IReadOnlyList<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var books = await repository.GetAllAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(searchTerm))
            return books;

        return [.. books.Where(b =>
            b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            b.Isbn.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))];
    }
}
