using System.Net;
using System.Net.Http.Json;
using Aspire.Hosting.Testing;
using BookStore.Core.Models;

namespace BookStore.IntegrationTests;

/// <summary>
/// Integration tests that start the full Aspire distributed application and call
/// the BookStore API directly, validating real HTTP responses end-to-end.
/// </summary>
public sealed class BooksApiIntegrationTests : IAsyncLifetime
{
    private DistributedApplicationFactory? _factory;
    private HttpClient? _client;

    public async ValueTask InitializeAsync()
    {
        _factory = new DistributedApplicationFactory(typeof(Projects.BookStore_AppHost));
        await _factory.StartAsync(TestContext.Current.CancellationToken);
        _client = _factory.CreateHttpClient("api");
    }

    public async ValueTask DisposeAsync()
    {
        _client?.Dispose();
        if (_factory is not null)
            await _factory.DisposeAsync();
    }

    // ── GET /api/books ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllBooks_ReturnsOk_WithSeededData()
    {
        var books = await _client!.GetFromJsonAsync<List<Book>>("/api/books",
            TestContext.Current.CancellationToken);

        Assert.NotNull(books);
        Assert.NotEmpty(books);
    }

    [Fact]
    public async Task GetAllBooks_ReturnsExpectedSeedBooks()
    {
        var books = await _client!.GetFromJsonAsync<List<Book>>("/api/books",
            TestContext.Current.CancellationToken);

        Assert.NotNull(books);
        Assert.Contains(books, b => b.Title == "The Pragmatic Programmer");
        Assert.Contains(books, b => b.Title == "Clean Code");
    }

    // ── GET /api/books/{id} ───────────────────────────────────────────────────

    [Fact]
    public async Task GetBookById_ExistingId_ReturnsOk()
    {
        var response = await _client!.GetAsync("/api/books/1",
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var book = await response.Content.ReadFromJsonAsync<Book>(
            TestContext.Current.CancellationToken);
        Assert.NotNull(book);
        Assert.Equal(1, book.Id);
    }

    [Fact]
    public async Task GetBookById_NonExistentId_ReturnsNotFound()
    {
        var response = await _client!.GetAsync("/api/books/99999",
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ── GET /api/books/search ─────────────────────────────────────────────────

    [Fact]
    public async Task SearchBooks_MatchingTerm_ReturnsFilteredResults()
    {
        var books = await _client!.GetFromJsonAsync<List<Book>>(
            "/api/books/search?q=Pragmatic",
            TestContext.Current.CancellationToken);

        Assert.NotNull(books);
        Assert.Contains(books, b => b.Title.Contains("Pragmatic", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task SearchBooks_NonMatchingTerm_ReturnsEmptyList()
    {
        var books = await _client!.GetFromJsonAsync<List<Book>>(
            "/api/books/search?q=xyzzy_does_not_exist",
            TestContext.Current.CancellationToken);

        Assert.NotNull(books);
        Assert.Empty(books);
    }

    // ── GET /api/books/category/{category} ───────────────────────────────────

    [Fact]
    public async Task GetBooksByCategory_Technology_ReturnsOnlyTechBooks()
    {
        var books = await _client!.GetFromJsonAsync<List<Book>>(
            "/api/books/category/Technology",
            TestContext.Current.CancellationToken);

        Assert.NotNull(books);
        Assert.NotEmpty(books);
        Assert.All(books, b => Assert.Equal(BookCategory.Technology, b.Category));
    }

    // ── POST /api/books ───────────────────────────────────────────────────────

    [Fact]
    public async Task AddBook_ValidBook_ReturnsCreated()
    {
        var newBook = new Book(0, "Test-Driven Development", "Kent Beck",
            "978-0321146533", 39.99m, BookCategory.Technology, 10,
            new DateOnly(2002, 11, 8));

        var response = await _client!.PostAsJsonAsync("/api/books", newBook,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Book>(
            TestContext.Current.CancellationToken);
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal("Test-Driven Development", created.Title);
    }

    [Fact]
    public async Task AddBook_EmptyTitle_ReturnsBadRequest()
    {
        var invalidBook = new Book(0, "", "Author", "isbn", 10m,
            BookCategory.Technology, 1, new DateOnly(2020, 1, 1));

        var response = await _client!.PostAsJsonAsync("/api/books", invalidBook,
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ── GET /api/books/{id}/stock ─────────────────────────────────────────────

    [Fact]
    public async Task GetStock_ExistingBook_ReturnsStockStatus()
    {
        var response = await _client!.GetAsync("/api/books/1/stock",
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
