using BookStore.Core.Exceptions;
using BookStore.Core.Models;
using BookStore.Core.Repositories;
using BookStore.Core.Services;
using BookStore.UnitTests.TestData;
using NSubstitute;

namespace BookStore.UnitTests;

/// <summary>
/// Theory-driven tests for <see cref="BookService"/> using xUnit v3.
///
/// Key concepts demonstrated:
///  - [Theory] with [InlineData] for parameterized, readable tests
///  - [Theory] with [MemberData] for complex/reusable data sets
///  - [Theory] with [ClassData] to share data across test files
///  - Testing boundary conditions (0%, 100%, negative discounts)
///  - TestContext.Current.CancellationToken (xUnit v3 cooperative cancellation)
/// </summary>
public sealed class BookServiceTheoryTests
{
    private readonly IBookRepository _repository;
    private readonly BookService _sut;

    public BookServiceTheoryTests()
    {
        _repository = Substitute.For<IBookRepository>();
        _sut = new BookService(_repository);
    }

    // ── Discount calculation — [InlineData] ───────────────────────────────────

    /// <summary>
    /// [InlineData] is ideal for a small, fixed set of values that fit
    /// comfortably on a single line.
    /// </summary>
    [Theory]
    [InlineData(10,  90.00)]   // 10% off £100
    [InlineData(25,  75.00)]   // 25% off £100
    [InlineData(50,  50.00)]   // 50% off £100
    [InlineData(0,  100.00)]   // No discount
    [InlineData(100,  0.00)]   // Fully free
    public async Task CalculateDiscountPriceAsync_ValidPercentage_ReturnsCorrectPrice(
        decimal discountPercentage, decimal expectedPrice)
    {
        var ct = TestContext.Current.CancellationToken;

        // Arrange — book with a fixed price of £100 for easy mental arithmetic
        var book = BookTestData.TechBook with { Price = 100.00m };
        _repository.GetByIdAsync(book.Id, ct).Returns(book);

        // Act
        var result = await _sut.CalculateDiscountPriceAsync(book.Id, discountPercentage, ct);

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    /// <summary>
    /// Boundary tests: percentages outside [0, 100] are always invalid.
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(-100)]
    [InlineData(200)]
    public async Task CalculateDiscountPriceAsync_InvalidPercentage_ThrowsArgumentOutOfRangeException(
        decimal discountPercentage)
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => _sut.CalculateDiscountPriceAsync(1, discountPercentage,
                TestContext.Current.CancellationToken));
    }

    // ── Discount calculation — [MemberData] ──────────────────────────────────

    /// <summary>
    /// [MemberData] references a static property or method, which is useful
    /// when the test data is shared, computed, or too verbose for inline use.
    /// </summary>
    [Theory]
    [MemberData(nameof(BookTestData.DiscountTestCases), MemberType = typeof(BookTestData))]
    public async Task CalculateDiscountPriceAsync_MemberData_ReturnsCorrectPrice(
        int bookId, decimal price, decimal discountPercentage, decimal expectedPrice)
    {
        var ct = TestContext.Current.CancellationToken;

        // Arrange
        var book = BookTestData.TechBook with { Id = bookId, Price = price };
        _repository.GetByIdAsync(bookId, ct).Returns(book);

        // Act
        var result = await _sut.CalculateDiscountPriceAsync(bookId, discountPercentage, ct);

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    // ── Validation — [ClassData] ──────────────────────────────────────────────

    /// <summary>
    /// [ClassData] wraps a <c>TheoryData&lt;T&gt;</c> class, enabling strong
    /// typing and the ability to reuse data across multiple test files.
    /// 
    /// This example demonstrates using <see cref="SerializableBook"/>, a test-specific
    /// wrapper that implements <see cref="IXunitSerializable"/> to enable Test Explorer
    /// enumeration without polluting the domain model with test framework dependencies.
    /// </summary>
    [Theory]
    [ClassData(typeof(InvalidBookClassData))]
    public async Task AddBookAsync_InvalidBook_ThrowsException(SerializableBook serializableBook)
    {
        var ct = TestContext.Current.CancellationToken;
        var invalidBook = serializableBook.Book;

        // No repository call should occur for invalid input
        await Assert.ThrowsAnyAsync<ArgumentException>(
            () => _sut.AddBookAsync(invalidBook, ct));

        await _repository.DidNotReceive()
                         .AddAsync(Arg.Any<Book>(), Arg.Any<CancellationToken>());
    }

    // ── Category search — [InlineData] ────────────────────────────────────────

    [Theory]
    [InlineData(BookCategory.Technology, 1, "The Pragmatic Programmer")]
    [InlineData(BookCategory.Science,    1, "A Brief History of Time")]
    public async Task GetBooksByCategoryAsync_KnownCategory_ReturnsMatchingBooks(
        BookCategory category, int expectedCount, string expectedTitle)
    {
        var ct = TestContext.Current.CancellationToken;

        // Arrange — filter by category from the shared catalog
        var filtered = BookTestData.AllBooks.Where(b => b.Category == category).ToList();
        _repository.GetByCategoryAsync(category, ct).Returns(filtered);

        // Act
        var result = await _sut.GetBooksByCategoryAsync(category, ct);

        // Assert
        Assert.Equal(expectedCount, result.Count);
        Assert.Equal(expectedTitle, result[0].Title);
    }

    // ── Not-found scenarios ───────────────────────────────────────────────────

    [Theory]
    [InlineData(0)]    // zero is not a valid book ID
    [InlineData(-1)]   // negative IDs never exist
    [InlineData(9999)] // non-existent positive ID
    public async Task CalculateDiscountPriceAsync_NonExistentBook_ThrowsBookNotFoundException(int bookId)
    {
        var ct = TestContext.Current.CancellationToken;
        _repository.GetByIdAsync(bookId, ct).Returns((Book?)null);

        var ex = await Assert.ThrowsAsync<BookNotFoundException>(
            () => _sut.CalculateDiscountPriceAsync(bookId, 10, ct));

        Assert.Equal(bookId, ex.BookId);
    }
}

