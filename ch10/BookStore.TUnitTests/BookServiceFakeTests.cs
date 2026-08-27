using BookStore.Core.Exceptions;
using BookStore.Core.Models;
using BookStore.Core.Services;
using BookStore.TUnitTests.Fakes;
using TUnit.Assertions.Extensions;

namespace BookStore.TUnitTests;

/// <summary>
/// Demonstrates testing <see cref="BookService"/> with a compile-time fake
/// (<see cref="FakeBookRepository"/>) instead of a runtime-generated proxy.
///
/// The fake is a plain partial class that implements the repository interface
/// using an in-memory dictionary.  Because no IL-emit or Castle proxy is
/// involved, the tests are fully Native-AOT-safe and the debugger can step
/// into the fake's implementation directly.
/// </summary>
public sealed class BookServiceFakeTests
{
    private FakeBookRepository _repository = null!;
    private BookService _sut = null!;

    [Before(Test)]
    public void Setup()
    {
        _repository = new FakeBookRepository();
        _sut = new BookService(_repository);
    }

    // ── AddBookAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task AddBookAsync_ValidBook_AssignsNewId()
    {
        var book = new Book(0, "Effective C#", "Bill Wagner", "978-0672337871",
            39.99m, BookCategory.Technology, 5, new DateOnly(2017, 3, 3));

        var result = await _sut.AddBookAsync(book);

        await Assert.That(result.Id).IsGreaterThan(0);
    }

    [Test]
    public async Task GetBookAsync_AfterAdd_ReturnsAddedBook()
    {
        var book = new Book(0, "Effective C#", "Bill Wagner", "978-0672337871",
            39.99m, BookCategory.Technology, 5, new DateOnly(2017, 3, 3));
        var added = await _sut.AddBookAsync(book);

        var found = await _sut.GetBookAsync(added.Id);

        await Assert.That(found).IsNotNull();
        await Assert.That(found!.Title).IsEqualTo(book.Title);
    }

    [Test]
    public async Task GetBookAsync_UnknownId_ReturnsNull()
    {
        var result = await _sut.GetBookAsync(999);

        await Assert.That(result).IsNull();
    }

    // ── UpdateBookAsync ───────────────────────────────────────────────────────

    [Test]
    public async Task UpdateBookAsync_ExistingBook_PersistsChanges()
    {
        var book = new Book(0, "Effective C#", "Bill Wagner", "978-0672337871",
            39.99m, BookCategory.Technology, 5, new DateOnly(2017, 3, 3));
        var added = await _sut.AddBookAsync(book);
        var updated = added with { Price = 29.99m };

        var result = await _sut.UpdateBookAsync(updated);

        await Assert.That(result.Price).IsEqualTo(29.99m);

        var retrieved = await _sut.GetBookAsync(added.Id);
        await Assert.That(retrieved!.Price).IsEqualTo(29.99m);
    }

    [Test]
    public async Task UpdateBookAsync_NonExistentBook_ThrowsBookNotFoundException()
    {
        var missing = new Book(999, "Missing", "Nobody", "978-0000000000",
            9.99m, BookCategory.Other, 0, new DateOnly(2020, 1, 1));

        await Assert.That(async () => await _sut.UpdateBookAsync(missing))
            .ThrowsExactly<BookNotFoundException>();
    }

    // ── RemoveBookAsync ───────────────────────────────────────────────────────

    [Test]
    public async Task RemoveBookAsync_ExistingBook_RemovesItFromStore()
    {
        var book = new Book(0, "Effective C#", "Bill Wagner", "978-0672337871",
            39.99m, BookCategory.Technology, 5, new DateOnly(2017, 3, 3));
        var added = await _sut.AddBookAsync(book);

        await _sut.RemoveBookAsync(added.Id);

        var found = await _sut.GetBookAsync(added.Id);
        await Assert.That(found).IsNull();
    }

    // ── SearchBooksAsync ──────────────────────────────────────────────────────

    [Test]
    [Arguments("Effective", 1)]
    [Arguments("Wagner",    1)]
    [Arguments("978-",      1)]
    [Arguments("zzz_none",  0)]
    [Arguments("",          1)]
    public async Task SearchBooksAsync_VariousTerms_ReturnsExpectedCount(
        string searchTerm, int expectedCount)
    {
        var book = new Book(0, "Effective C#", "Bill Wagner", "978-0672337871",
            39.99m, BookCategory.Technology, 5, new DateOnly(2017, 3, 3));
        await _sut.AddBookAsync(book);

        var results = await _sut.SearchBooksAsync(searchTerm);

        await Assert.That(results.Count).IsEqualTo(expectedCount);
    }

    // ── Seeded repository ─────────────────────────────────────────────────────

    [Test]
    public async Task GetAllBooksAsync_SeededRepository_ReturnsAllBooks()
    {
        var seeded = new FakeBookRepository(SampleBooks.All);
        var sut = new BookService(seeded);

        var all = await sut.GetAllBooksAsync();

        await Assert.That(all.Count).IsEqualTo(SampleBooks.All.Count);
    }

    [Test]
    public async Task GetBooksByCategoryAsync_SeededRepository_FiltersByCategory()
    {
        var seeded = new FakeBookRepository(SampleBooks.All);
        var sut = new BookService(seeded);

        var techBooks = await sut.GetBooksByCategoryAsync(BookCategory.Technology);

        await Assert.That(techBooks).IsNotEmpty();
        await Assert.That(techBooks.All(b => b.Category == BookCategory.Technology)).IsTrue();
    }
}
