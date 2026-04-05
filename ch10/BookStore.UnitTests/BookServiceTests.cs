using BookStore.Core.Exceptions;
using BookStore.Core.Models;
using BookStore.Core.Repositories;
using BookStore.Core.Services;
using BookStore.UnitTests.TestData;
using NSubstitute;

namespace BookStore.UnitTests;

/// <summary>
/// Unit tests for <see cref="BookService"/> using xUnit v3 with the
/// Microsoft Testing Platform runner.
///
/// Key concepts demonstrated:
///  - [Fact] for simple pass/fail assertions
///  - Using NSubstitute to create lightweight repository stubs
///  - Testing both the happy path and exception scenarios
///  - Async test methods (xUnit v3 supports async natively)
///  - TestContext.Current.CancellationToken for cooperative cancellation (xUnit v3)
/// </summary>
public sealed class BookServiceTests
{
    private readonly IBookRepository _repository;
    private readonly BookService _sut;

    public BookServiceTests()
    {
        // NSubstitute creates a stand-in for IBookRepository.
        // The service under test never touches a real database.
        _repository = Substitute.For<IBookRepository>();
        _sut = new BookService(_repository);
    }

    // ── GetBookAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetBookAsync_ExistingId_ReturnsBook()
    {
        // xUnit v3 exposes a per-test CancellationToken via TestContext.Current.
        // Pass it to all async calls so the test honors framework-level cancellation.
        var ct = TestContext.Current.CancellationToken;

        // Arrange
        _repository.GetByIdAsync(1, ct).Returns(BookTestData.TechBook);

        // Act
        var result = await _sut.GetBookAsync(1, ct);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(BookTestData.TechBook, result);
    }

    [Fact]
    public async Task GetBookAsync_NonExistentId_ReturnsNull()
    {
        var ct = TestContext.Current.CancellationToken;

        // Arrange
        _repository.GetByIdAsync(99, ct).Returns((Book?)null);

        // Act
        var result = await _sut.GetBookAsync(99, ct);

        // Assert
        Assert.Null(result);
    }

    // ── AddBookAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task AddBookAsync_ValidBook_DelegatesToRepository()
    {
        var ct = TestContext.Current.CancellationToken;

        // Arrange
        var newBook = BookTestData.TechBook with { Id = 0 };
        var savedBook = newBook with { Id = 42 };

        _repository.AddAsync(newBook, ct).Returns(savedBook);

        // Act
        var result = await _sut.AddBookAsync(newBook, ct);

        // Assert
        Assert.Equal(42, result.Id);
        await _repository.Received(1).AddAsync(newBook, ct);
    }

    [Fact]
    public async Task AddBookAsync_NullBook_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.AddBookAsync(null!, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task AddBookAsync_EmptyTitle_ThrowsArgumentException()
    {
        var invalid = BookTestData.TechBook with { Id = 0, Title = "" };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.AddBookAsync(invalid, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task AddBookAsync_NegativePrice_ThrowsArgumentException()
    {
        var invalid = BookTestData.TechBook with { Id = 0, Price = -5m };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.AddBookAsync(invalid, TestContext.Current.CancellationToken));
    }

    // ── UpdateBookAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateBookAsync_ExistingBook_ReturnsUpdated()
    {
        var ct = TestContext.Current.CancellationToken;

        // Arrange
        var updated = BookTestData.TechBook with { Price = 39.99m };
        _repository.ExistsAsync(updated.Id, ct).Returns(true);
        _repository.UpdateAsync(updated, ct).Returns(updated);

        // Act
        var result = await _sut.UpdateBookAsync(updated, ct);

        // Assert
        Assert.Equal(39.99m, result.Price);
    }

    [Fact]
    public async Task UpdateBookAsync_NonExistentBook_ThrowsBookNotFoundException()
    {
        var ct = TestContext.Current.CancellationToken;

        // Arrange
        _repository.ExistsAsync(Arg.Any<int>(), ct).Returns(false);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BookNotFoundException>(
            () => _sut.UpdateBookAsync(BookTestData.TechBook, ct));

        Assert.Equal(BookTestData.TechBook.Id, ex.BookId);
    }

    // ── RemoveBookAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task RemoveBookAsync_ExistingId_CallsRepositoryDelete()
    {
        var ct = TestContext.Current.CancellationToken;

        // Arrange
        _repository.ExistsAsync(1, ct).Returns(true);

        // Act
        await _sut.RemoveBookAsync(1, ct);

        // Assert
        await _repository.Received(1).DeleteAsync(1, ct);
    }

    [Fact]
    public async Task RemoveBookAsync_NonExistentId_ThrowsBookNotFoundException()
    {
        var ct = TestContext.Current.CancellationToken;
        _repository.ExistsAsync(Arg.Any<int>(), ct).Returns(false);

        await Assert.ThrowsAsync<BookNotFoundException>(
            () => _sut.RemoveBookAsync(99, ct));
    }

    // ── IsInStockAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task IsInStockAsync_BookWithStock_ReturnsTrue()
    {
        var ct = TestContext.Current.CancellationToken;
        _repository.GetByIdAsync(1, ct).Returns(BookTestData.TechBook); // StockQuantity = 10

        var result = await _sut.IsInStockAsync(1, ct);

        Assert.True(result);
    }

    [Fact]
    public async Task IsInStockAsync_BookOutOfStock_ReturnsFalse()
    {
        var ct = TestContext.Current.CancellationToken;
        _repository.GetByIdAsync(3, ct).Returns(BookTestData.OutOfStockBook); // StockQuantity = 0

        var result = await _sut.IsInStockAsync(3, ct);

        Assert.False(result);
    }

    [Fact]
    public async Task IsInStockAsync_NonExistentBook_ThrowsBookNotFoundException()
    {
        var ct = TestContext.Current.CancellationToken;
        _repository.GetByIdAsync(99, ct).Returns((Book?)null);

        await Assert.ThrowsAsync<BookNotFoundException>(() => _sut.IsInStockAsync(99, ct));
    }

    // ── SearchBooksAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task SearchBooksAsync_EmptyTerm_ReturnsAllBooks()
    {
        var ct = TestContext.Current.CancellationToken;
        _repository.GetAllAsync(ct).Returns(BookTestData.AllBooks);

        var result = await _sut.SearchBooksAsync("", ct);

        Assert.Equal(BookTestData.AllBooks.Count, result.Count);
    }

    [Fact]
    public async Task SearchBooksAsync_MatchingTitle_ReturnsFilteredBooks()
    {
        var ct = TestContext.Current.CancellationToken;
        _repository.GetAllAsync(ct).Returns(BookTestData.AllBooks);

        var result = await _sut.SearchBooksAsync("Pragmatic", ct);

        Assert.Single(result);
        Assert.Equal("The Pragmatic Programmer", result[0].Title);
    }

    [Fact]
    public async Task SearchBooksAsync_CaseInsensitive_FindsBook()
    {
        var ct = TestContext.Current.CancellationToken;
        _repository.GetAllAsync(ct).Returns(BookTestData.AllBooks);

        var result = await _sut.SearchBooksAsync("HAWKING", ct);

        Assert.Single(result);
        Assert.Equal("Stephen Hawking", result[0].Author);
    }
}
