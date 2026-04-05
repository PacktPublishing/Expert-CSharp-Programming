using BookStore.Api.Repositories;
using BookStore.Core.Models;
using BookStore.Core.Repositories;
using BookStore.Core.Services;
using BookStore.Web.Components;
using Microsoft.AspNetCore.Builder;
using TUnit.Core.Interfaces;

namespace BookStore.E2ETests.Infrastructure;

/// <summary>
/// A test fixture that starts both the BookStore <strong>API</strong> and the Blazor Server
/// <strong>Web</strong> applications on random Kestrel ports so Playwright UI tests can
/// exercise the full request flow.
///
/// <list type="bullet">
///   <item>The API server hosts all REST endpoints backed by an in-memory repository.</item>
///   <item>The Web server hosts the Blazor Server application; its <c>HttpClient</c>
///         is pointed at the API server's URL.</item>
/// </list>
/// </summary>
public sealed class FullStackApplicationFactory : IAsyncInitializer, IAsyncDisposable
{
    private WebApplication? _apiApp;
    private WebApplication? _webApp;

    /// <summary>The base URL of the BookStore API server (e.g. <c>http://127.0.0.1:PORT</c>).</summary>
    public string ApiBaseUrl { get; private set; } = string.Empty;

    /// <summary>The base URL of the Blazor Web server (e.g. <c>http://127.0.0.1:PORT</c>).</summary>
    public string WebBaseUrl { get; private set; } = string.Empty;

    // ── IAsyncInitializer ─────────────────────────────────────────────────────

    public async Task InitializeAsync()
    {
        // ── 1. Start BookStore.Api ────────────────────────────────────────────
        var apiBuilder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Testing",
            ApplicationName = "BookStore.Api",
        });

        apiBuilder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
        apiBuilder.Services.AddScoped<IBookService, BookService>();
        apiBuilder.WebHost.UseUrls("http://127.0.0.1:0");

        _apiApp = apiBuilder.Build();
        MapBooksApi(_apiApp);

        await _apiApp.StartAsync();
        ApiBaseUrl = _apiApp.Urls.First();

        // ── 2. Start BookStore.Web (Blazor Server) ────────────────────────────
        var webAssemblyLocation = Path.GetDirectoryName(typeof(App).Assembly.Location)!;
        var webBuilder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Testing",
            ApplicationName = "BookStore.Web",
            ContentRootPath = webAssemblyLocation,
        });

        webBuilder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        webBuilder.Services.AddHttpClient("BookStoreApi", client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
        });

        webBuilder.WebHost.UseUrls("http://127.0.0.1:0");

        _webApp = webBuilder.Build();
        _webApp.UseStaticFiles();
        _webApp.UseAntiforgery();
        _webApp.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        await _webApp.StartAsync();
        WebBaseUrl = _webApp.Urls.First();
    }

    // ── IAsyncDisposable ──────────────────────────────────────────────────────

    public async ValueTask DisposeAsync()
    {
        if (_webApp is not null)
        {
            await _webApp.StopAsync();
            await _webApp.DisposeAsync();
        }

        if (_apiApp is not null)
        {
            await _apiApp.StopAsync();
            await _apiApp.DisposeAsync();
        }
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private static void MapBooksApi(WebApplication app)
    {
        var books = app.MapGroup("/api/books").WithTags("Books");

        books.MapGet("/",
            async (IBookService service) => Results.Ok(await service.GetAllBooksAsync()));

        books.MapGet("/{id:int}", async (int id, IBookService service) =>
        {
            var book = await service.GetBookAsync(id);
            return book is null ? Results.NotFound() : Results.Ok(book);
        });

        books.MapGet("/category/{category}", async (BookCategory category, IBookService service) =>
            Results.Ok(await service.GetBooksByCategoryAsync(category)));

        books.MapGet("/search", async (string? q, IBookService service) =>
            Results.Ok(await service.SearchBooksAsync(q ?? string.Empty)));
    }
}
