using BookStore.E2ETests.Infrastructure;
using Microsoft.Playwright;
using TUnit.Playwright;

namespace BookStore.E2ETests;

/// <summary>
/// API-level E2E tests for the BookStore API endpoints.
///
/// These tests use Playwright's <c>APIRequestContext</c> to call the REST API
/// over real HTTP.  They complement the UI tests by verifying that the API
/// contract (status codes, JSON shape, error responses) is correct without
/// needing a browser.
///
/// Key concepts demonstrated:
///  - Playwright's APIRequestContext for headless HTTP testing
///  - Verifying JSON response bodies
///  - Testing error scenarios (404, 400)
///  - Sharing a single WebApplicationFactory across the class with [ClassDataSource]
/// </summary>
[ClassDataSource<BookStoreWebApplicationFactory>(Shared = SharedType.PerClass)]
public sealed class BooksApiTests(BookStoreWebApplicationFactory factory) : PageTest
{
    // Per-test API context — created fresh for each test to avoid shared state.
    private IAPIRequestContext? _api;

    [Before(Test)]
    public async Task CreateApiContextAsync()
    {
        _api = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = factory.BaseUrl,
        });
    }

    [After(Test)]
    public async Task DisposeApiContextAsync()
    {
        if (_api is not null)
            await _api.DisposeAsync();
    }

    // ── GET /api/books ────────────────────────────────────────────────────────

    [Test]
    public async Task GetAllBooks_Returns200WithBooks()
    {
        var response = await _api!.GetAsync("/api/books");

        await Assert.That(response.Status).IsEqualTo(200);

        var body = await response.JsonAsync();
        await Assert.That(body).IsNotNull();
    }

    [Test]
    public async Task GetAllBooks_ResponseContainsSeedData()
    {
        var response = await _api!.GetAsync("/api/books");
        var json = await response.TextAsync();

        // The seeded catalog includes "The Pragmatic Programmer"
        await Assert.That(json).Contains("Pragmatic");
    }

    // ── GET /api/books/{id} ───────────────────────────────────────────────────

    [Test]
    public async Task GetBookById_ExistingId_Returns200()
    {
        var response = await _api!.GetAsync("/api/books/1");

        await Assert.That(response.Status).IsEqualTo(200);
    }

    [Test]
    public async Task GetBookById_ExistingId_ReturnsCorrectBook()
    {
        var response = await _api!.GetAsync("/api/books/1");
        var json = await response.TextAsync();

        await Assert.That(json).Contains("Pragmatic");
    }

    [Test]
    public async Task GetBookById_NonExistentId_Returns404()
    {
        var response = await _api!.GetAsync("/api/books/9999");

        await Assert.That(response.Status).IsEqualTo(404);
    }

    // ── GET /api/books/search ─────────────────────────────────────────────────

    [Test]
    public async Task SearchBooks_MatchingTerm_ReturnsResults()
    {
        var response = await _api!.GetAsync("/api/books/search?q=Hawking");

        await Assert.That(response.Status).IsEqualTo(200);
        var json = await response.TextAsync();
        await Assert.That(json).Contains("Hawking");
    }

    [Test]
    public async Task SearchBooks_NonMatchingTerm_ReturnsEmptyArray()
    {
        var response = await _api!.GetAsync("/api/books/search?q=zzz_nothing_matches_this");

        await Assert.That(response.Status).IsEqualTo(200);

        // Should be an empty JSON array "[]"
        var json = await response.TextAsync();
        await Assert.That(json.Trim()).IsEqualTo("[]");
    }

    // ── GET /api/books/{id}/stock ─────────────────────────────────────────────

    [Test]
    public async Task GetStockStatus_BookInStock_ReturnsTrue()
    {
        var response = await _api!.GetAsync("/api/books/1/stock");

        await Assert.That(response.Status).IsEqualTo(200);
        var json = await response.TextAsync();
        await Assert.That(json).Contains("true");
    }

    // ── GET /api/books/{id}/discount ──────────────────────────────────────────

    [Test]
    public async Task GetDiscount_ValidPercentage_ReturnsDiscountedPrice()
    {
        var response = await _api!.GetAsync("/api/books/1/discount?percentage=10");

        await Assert.That(response.Status).IsEqualTo(200);
        var json = await response.TextAsync();
        // ASP.NET Core serialises property names as camelCase by default
        await Assert.That(json).Contains("discountedPrice");
    }

    [Test]
    public async Task GetDiscount_InvalidPercentage_Returns400()
    {
        var response = await _api!.GetAsync("/api/books/1/discount?percentage=150");

        await Assert.That(response.Status).IsEqualTo(400);
    }

    // ── GET /api/books/category/{category} ───────────────────────────────────

    [Test]
    public async Task GetByCategory_Technology_ReturnsTechBooks()
    {
        var response = await _api!.GetAsync("/api/books/category/Technology");

        await Assert.That(response.Status).IsEqualTo(200);
        var json = await response.TextAsync();
        // The seeded data contains "The Pragmatic Programmer" and "Clean Code" in Technology
        await Assert.That(json).Contains("Pragmatic");
    }
}

