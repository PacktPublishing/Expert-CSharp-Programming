using BookStore.Core.Models;
using BookStore.Core.Repositories;
using BookStore.Core.Services;
using TUnit.Mocks;

namespace BookStore.TUnitTests;

/// <summary>
/// Demonstrates TUnit's data-driven test attributes:
///  - [Arguments] — equivalent to xUnit's [InlineData]
///  - [MethodDataSource] — equivalent to [MemberData]
///  - [ClassDataSource] — DI-managed shared instances injected by TUnit
///
/// TUnit allows data-driven tests with full async support and expressive
/// fluent assertions through its built-in <c>Assert.That</c> API.
/// </summary>
public sealed class BookServiceDataDrivenTests
{
    // ── [Arguments] — inline data ─────────────────────────────────────────────

    /// <summary>
    /// [Arguments] is TUnit's inline data attribute, identical in spirit to
    /// xUnit's [InlineData].  Each invocation is an independent test run.
    /// </summary>
    [Test]
    [Arguments(10,  90.00)]
    [Arguments(25,  75.00)]
    [Arguments(50,  50.00)]
    [Arguments(0,  100.00)]
    [Arguments(100,  0.00)]
    public async Task CalculateDiscount_InlineArguments_ReturnsCorrectPrice(
        decimal discountPercentage, decimal expectedPrice)
    {
        var repository = Mock.Of<IBookRepository>();
        var sut = new BookService(repository.Object);

        var book = SampleBooks.TechBook with { Price = 100.00m };
        repository.GetByIdAsync(book.Id, Any()).Returns(book);

        var result = await sut.CalculateDiscountPriceAsync(book.Id, discountPercentage);

        await Assert.That(result).IsEqualTo(expectedPrice);
    }

    // ── [MethodDataSource] — member data ─────────────────────────────────────

    /// <summary>
    /// [MethodDataSource] is TUnit's equivalent of xUnit's [MemberData].
    /// It references a static method (or property) that returns
    /// <c>IEnumerable</c> of the argument types.
    /// </summary>
    [Test]
    [MethodDataSource(nameof(SearchTermData))]
    public async Task SearchBooks_MethodDataSource_ReturnsMatchingResults(
        string searchTerm, int expectedCount)
    {
        var repository = Mock.Of<IBookRepository>();
        var sut = new BookService(repository.Object);
        repository.GetAllAsync(Any()).Returns(SampleBooks.All);

        var results = await sut.SearchBooksAsync(searchTerm);

        await Assert.That(results.Count).IsEqualTo(expectedCount);
    }

    // Called by [MethodDataSource] — must be public static
    public static IEnumerable<(string searchTerm, int expectedCount)> SearchTermData()
    {
        yield return ("Pragmatic", 1);   // matches TechBook by title
        yield return ("Hawking",   1);   // matches ScienceBook by author
        yield return ("978-",      3);   // all ISBNs start with 978-
        yield return ("zzz_none",  0);   // no matches
        yield return ("",          3);   // blank → all books
    }

    // ── [ClassDataSource] — DI-injected shared data ───────────────────────────

    /// <summary>
    /// [ClassDataSource] lets TUnit create and inject an instance of a class
    /// that implements the test data.  This is useful for heavyweight fixtures
    /// (e.g. database seeds) that should be created once and reused.
    /// Here we use a lightweight in-memory catalogue as the data source.
    /// </summary>
    [Test]
    [ClassDataSource<BooksDataSource>(Shared = SharedType.PerClass)]
    public async Task GetBooksByCategoryAsync_ClassDataSource_ReturnsTechnologyBooks(
        BooksDataSource data)
    {
        var repository = Mock.Of<IBookRepository>();
        var sut = new BookService(repository.Object);
        repository.GetByCategoryAsync(BookCategory.Technology, Any())
                  .Returns(data.TechBooks);

        var results = await sut.GetBooksByCategoryAsync(BookCategory.Technology);

        await Assert.That(results).IsNotEmpty();
        await Assert.That(results.All(b => b.Category == BookCategory.Technology)).IsTrue();
    }
}

/// <summary>
/// Shared data source for <see cref="BookServiceDataDrivenTests"/>.
/// TUnit creates this once per class (due to <c>Shared = SharedType.PerClass</c>)
/// and injects it into the test via the <c>[ClassDataSource]</c> attribute.
/// </summary>
public sealed class BooksDataSource
{
    public IReadOnlyList<Book> TechBooks { get; } =
    [
        new(1, "The Pragmatic Programmer", "David Thomas", "978-0135957059",
            49.99m, BookCategory.Technology, 10, new DateOnly(2019, 9, 13)),
        new(2, "Clean Code", "Robert C. Martin", "978-0132350884",
            44.99m, BookCategory.Technology, 8, new DateOnly(2008, 8, 1)),
    ];
}
