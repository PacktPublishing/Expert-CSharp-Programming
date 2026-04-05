using BookStore.Core.Exceptions;
using BookStore.Core.Models;
using BookStore.Core.Repositories;
using BookStore.Core.Services;
using TUnit.Assertions.Extensions;
using TUnit.Mocks;

namespace BookStore.TUnitTests;

/// <summary>
/// Demonstrates TUnit's mocking approach via TUnit.Mocks for <see cref="BookService"/>.
///
/// TUnit is built natively on the Microsoft Testing Platform (no VSTest shim),
/// supports fully async tests and hooks, and integrates cleanly with TUnit.Mocks —
/// a source-generated, AOT-compatible mocking framework.
///
/// Key TUnit concepts:
///  - [Test] attribute (equivalent to [Fact] / [Test] in xUnit/NUnit)
///  - [Before(Test)] / [After(Test)] lifecycle hooks
///  - Parallel test execution by default
///  - TUnit.Assertions fluent assertion API (Assert.That(...).IsEqualTo(...))
/// </summary>
public sealed class BookServiceMockTests
{
    // Each test instance gets its own mock so tests stay independent.
    private Mock<IBookRepository> _repository = null!;
    private BookService _sut = null!;

    [Before(Test)]
    public void Setup()
    {
        _repository = Mock.Of<IBookRepository>();
        _sut = new BookService(_repository.Object);
    }

    // ── GetBookAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task GetBookAsync_ExistingId_ReturnsCorrectBook()
    {
        var book = SampleBooks.TechBook;
        _repository.GetByIdAsync(book.Id, Any()).Returns(book);

        var result = await _sut.GetBookAsync(book.Id);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Title).IsEqualTo(book.Title);
    }

    [Test]
    public async Task GetBookAsync_NonExistentId_ReturnsNull()
    {
        _repository.GetByIdAsync(99, Any()).Returns((Book?)null);

        var result = await _sut.GetBookAsync(99);

        await Assert.That(result).IsNull();
    }

    // ── AddBookAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task AddBookAsync_ValidBook_ReturnsPersistedEntity()
    {
        var incoming = SampleBooks.TechBook with { Id = 0 };
        var persisted = incoming with { Id = 7 };
        _repository.AddAsync(incoming, Any()).Returns(persisted);

        var result = await _sut.AddBookAsync(incoming);

        await Assert.That(result.Id).IsEqualTo(7);
        _repository.AddAsync(incoming, Any()).WasCalled(Times.Once);
    }

    [Test]
    public async Task AddBookAsync_NullBook_ThrowsArgumentNullException()
    {
        await Assert.That(async () => await _sut.AddBookAsync(null!))
            .ThrowsException()
            .WithMessageContaining("book");
    }

    [Test]
    public async Task AddBookAsync_WhitespaceTitleBook_ThrowsArgumentException()
    {
        var invalid = SampleBooks.TechBook with { Id = 0, Title = "   " };

        await Assert.That(async () => await _sut.AddBookAsync(invalid))
            .ThrowsException()
            .WithMessageContaining("title");
    }

    [Test]
    public async Task AddBookAsync_NegativePriceBook_ThrowsArgumentException()
    {
        var invalid = SampleBooks.TechBook with { Id = 0, Price = -0.01m };

        await Assert.That(async () => await _sut.AddBookAsync(invalid))
            .ThrowsException()
            .WithMessageContaining("price");
    }

    // ── UpdateBookAsync ───────────────────────────────────────────────────────

    [Test]
    public async Task UpdateBookAsync_ExistingBook_DelegatesToRepository()
    {
        var updated = SampleBooks.TechBook with { Price = 34.99m };
        _repository.ExistsAsync(updated.Id, Any()).Returns(true);
        _repository.UpdateAsync(updated, Any()).Returns(updated);

        var result = await _sut.UpdateBookAsync(updated);

        await Assert.That(result.Price).IsEqualTo(34.99m);
        _repository.UpdateAsync(updated, Any()).WasCalled(Times.Once);
    }

    [Test]
    public async Task UpdateBookAsync_NonExistentBook_ThrowsBookNotFoundException()
    {
        _repository.ExistsAsync(Any(), Any()).Returns(false);

        await Assert.That(async () => await _sut.UpdateBookAsync(SampleBooks.TechBook))
            .ThrowsExactly<BookNotFoundException>();
    }

    // ── RemoveBookAsync ───────────────────────────────────────────────────────

    [Test]
    public async Task RemoveBookAsync_ExistingBook_CallsDelete()
    {
        _repository.ExistsAsync(SampleBooks.TechBook.Id, Any())
                   .Returns(true);

        await _sut.RemoveBookAsync(SampleBooks.TechBook.Id);

        _repository.DeleteAsync(SampleBooks.TechBook.Id, Any()).WasCalled(Times.Once);
    }

    [Test]
    public async Task RemoveBookAsync_NonExistentBook_ThrowsBookNotFoundException()
    {
        _repository.ExistsAsync(Any(), Any()).Returns(false);

        await Assert.That(async () => await _sut.RemoveBookAsync(404))
            .ThrowsExactly<BookNotFoundException>();
    }

    // ── IsInStockAsync ────────────────────────────────────────────────────────

    [Test]
    public async Task IsInStockAsync_BookWithPositiveStock_ReturnsTrue()
    {
        _repository.GetByIdAsync(SampleBooks.TechBook.Id, Any())
                   .Returns(SampleBooks.TechBook); // StockQuantity = 10

        var inStock = await _sut.IsInStockAsync(SampleBooks.TechBook.Id);

        await Assert.That(inStock).IsTrue();
    }

    [Test]
    public async Task IsInStockAsync_OutOfStockBook_ReturnsFalse()
    {
        _repository.GetByIdAsync(SampleBooks.OutOfStockBook.Id, Any())
                   .Returns(SampleBooks.OutOfStockBook); // StockQuantity = 0

        var inStock = await _sut.IsInStockAsync(SampleBooks.OutOfStockBook.Id);

        await Assert.That(inStock).IsFalse();
    }

    // ── CalculateDiscountPriceAsync ───────────────────────────────────────────

    [Test]
    public async Task CalculateDiscountPriceAsync_TenPercentOff_ReturnsCorrectPrice()
    {
        var book = SampleBooks.TechBook with { Price = 50.00m };
        _repository.GetByIdAsync(book.Id, Any()).Returns(book);

        var discounted = await _sut.CalculateDiscountPriceAsync(book.Id, 10);

        await Assert.That(discounted).IsEqualTo(45.00m);
    }

    [Test]
    public async Task CalculateDiscountPriceAsync_NegativePercentage_ThrowsArgumentOutOfRangeException()
    {
        await Assert.That(async () =>
            await _sut.CalculateDiscountPriceAsync(SampleBooks.TechBook.Id, -5))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }

    // ── SearchBooksAsync ──────────────────────────────────────────────────────

    [Test]
    public async Task SearchBooksAsync_BlankTerm_ReturnsAllBooks()
    {
        _repository.GetAllAsync(Any()).Returns(SampleBooks.All);

        var results = await _sut.SearchBooksAsync("  ");

        await Assert.That(results.Count).IsEqualTo(SampleBooks.All.Count);
    }

    [Test]
    public async Task SearchBooksAsync_MatchingAuthor_ReturnsOnlyMatches()
    {
        _repository.GetAllAsync(Any()).Returns(SampleBooks.All);

        var results = await _sut.SearchBooksAsync("Thomas");

        await Assert.That(results.Count).IsEqualTo(1);
        await Assert.That(results[0].Author).Contains("Thomas");
    }
}
