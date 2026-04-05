using BookStore.E2ETests.Infrastructure;
using TUnit.Playwright;

namespace BookStore.E2ETests;

/// <summary>
/// End-to-end UI tests for the BookStore catalog page using Playwright.
///
/// Key concepts demonstrated:
///  - TUnit.Playwright's <see cref="PageTest"/> base class provides a fully
///    configured <see cref="IPage"/> instance for each test
///  - <see cref="FullStackApplicationFactory"/> starts both the Blazor Web app
///    and the API backend on Kestrel so Playwright's browser can connect over
///    a real HTTP endpoint
///  - TUnit's [ClassDataSource] injects the shared server fixture once
///    per class to avoid repeated start/stop overhead
///  - Playwright's auto-waiting and Expect (soft assertions) API
/// </summary>
[ClassDataSource<FullStackApplicationFactory>(Shared = SharedType.PerClass)]
public sealed class BookCatalogPageTests(FullStackApplicationFactory factory) : PageTest
{
    // ── Catalog page loads ────────────────────────────────────────────────────

    [Test]
    public async Task CatalogPage_WhenLoaded_DisplaysPageTitle()
    {
        await Page.GotoAsync(factory.WebBaseUrl);

        await Expect(Page).ToHaveTitleAsync("BookStore - Catalog");
    }

    [Test]
    public async Task CatalogPage_WhenLoaded_ShowsBooksTable()
    {
        await Page.GotoAsync(factory.WebBaseUrl);

        var table = Page.Locator("#books-table");
        await Expect(table).ToBeVisibleAsync();
    }

    [Test]
    public async Task CatalogPage_WhenLoaded_ShowsSeedBooks()
    {
        await Page.GotoAsync(factory.WebBaseUrl);

        // The in-memory repository seeds 5 books; verify at least 3 rows
        var rows = Page.Locator("#books-table tbody tr");
        var count = await rows.CountAsync();
        await Assert.That(count).IsGreaterThanOrEqualTo(3);
    }

    // ── Search functionality ──────────────────────────────────────────────────

    [Test]
    public async Task CatalogPage_SearchByTitle_FiltersResults()
    {
        await Page.GotoAsync(factory.WebBaseUrl);

        // Type a search term and submit the form; Blazor navigates to /?q=Pragmatic
        await Page.Locator("input[type=search]").FillAsync("Pragmatic");
        await Page.Locator("button[type=submit]").ClickAsync();
        await Page.WaitForURLAsync(url => url.Contains("q=Pragmatic"));

        // Only books whose title/author/ISBN contains "Pragmatic" should be shown
        var rows = Page.Locator("#books-table tbody tr");
        await Expect(rows).ToHaveCountAsync(1);

        var firstTitle = await Page.Locator("#books-table tbody tr:first-child td:first-child")
                                    .InnerTextAsync();
        await Assert.That(firstTitle).Contains("Pragmatic");
    }

    [Test]
    public async Task CatalogPage_SearchWithNoResults_ShowsNoResultsMessage()
    {
        await Page.GotoAsync($"{factory.WebBaseUrl}?q=zzz_this_book_does_not_exist");

        await Expect(Page.Locator("#no-results")).ToBeVisibleAsync();
    }

    [Test]
    public async Task CatalogPage_SearchCleared_ShowsAllBooks()
    {
        // First search for something specific
        await Page.GotoAsync($"{factory.WebBaseUrl}?q=Pragmatic");
        await Expect(Page.Locator("#books-table")).ToBeVisibleAsync();
        var filteredRows = await Page.Locator("#books-table tbody tr").CountAsync();

        // Then clear the search to see all books
        await Page.GotoAsync(factory.WebBaseUrl);
        await Expect(Page.Locator("#books-table")).ToBeVisibleAsync();
        var allRows = await Page.Locator("#books-table tbody tr").CountAsync();

        await Assert.That(allRows).IsGreaterThan(filteredRows);
    }

    // ── Individual row data ───────────────────────────────────────────────────

    [Test]
    public async Task CatalogPage_TechBook_HasCorrectDetails()
    {
        await Page.GotoAsync($"{factory.WebBaseUrl}?q=Pragmatic");
        await Expect(Page.Locator("#books-table")).ToBeVisibleAsync();

        var row = Page.Locator("#books-table tbody tr").First;

        // Title column
        var title = await row.Locator("td:nth-child(1)").InnerTextAsync();
        await Assert.That(title).Contains("Pragmatic");

        // Author column
        var author = await row.Locator("td:nth-child(2)").InnerTextAsync();
        await Assert.That(author).IsNotEmpty();

        // Category column
        var category = await row.Locator("td:nth-child(3)").InnerTextAsync();
        await Assert.That(category).IsEqualTo("Technology");
    }
}
