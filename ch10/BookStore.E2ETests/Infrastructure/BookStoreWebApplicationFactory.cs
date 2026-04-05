using BookStore.Api.Repositories;
using BookStore.Core.Models;
using BookStore.Core.Repositories;
using BookStore.Core.Services;
using Microsoft.AspNetCore.Builder;
using TUnit.Core.Interfaces;

namespace BookStore.E2ETests.Infrastructure;

/// <summary>
/// A self-contained web server that starts the BookStore <strong>API</strong> application
/// on a random Kestrel port so Playwright tests can call it over real HTTP.
///
/// TUnit creates and initialises this class once per test class (due to
/// <c>Shared = SharedType.PerClass</c>) and injects it via <c>[ClassDataSource]</c>.
/// The <see cref="IAsyncInitializer"/> interface tells TUnit to call
/// <see cref="InitializeAsync"/> before the first test runs.
/// </summary>
public sealed class BookStoreWebApplicationFactory : IAsyncInitializer, IAsyncDisposable
{
    private WebApplication? _app;

    /// <summary>
    /// The base URL (e.g. <c>http://127.0.0.1:PORT</c>) of the API server.
    /// Populated during <see cref="InitializeAsync"/>.
    /// </summary>
    public string BaseUrl { get; private set; } = string.Empty;

    // ── IAsyncInitializer ─────────────────────────────────────────────────────

    /// <summary>
    /// Builds and starts the BookStore API on a random Kestrel port.
    /// Called automatically by TUnit before the first test in the shared scope.
    /// </summary>
    public async Task InitializeAsync()
    {
        var contentRoot = Path.GetDirectoryName(typeof(BookStore.Api.Repositories.InMemoryBookRepository).Assembly.Location)!;
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Testing",
            ApplicationName = "BookStore.Api",
            ContentRootPath = contentRoot,
        });

        // Register the same services as BookStore.Api/Program.cs
        builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
        builder.Services.AddScoped<IBookService, BookService>();

        // Bind Kestrel to port 0 so the OS assigns a free port automatically.
        builder.WebHost.UseUrls("http://127.0.0.1:0");

        _app = builder.Build();

        // ── Books API endpoints ───────────────────────────────────────────────
        MapBooksApi(_app);

        await _app.StartAsync();

        // The URL with the actual assigned port (e.g. http://127.0.0.1:54321)
        BaseUrl = _app.Urls.First();
    }

    // ── IAsyncDisposable ──────────────────────────────────────────────────────

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
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

        books.MapGet("/{id:int}/stock", async (int id, IBookService service) =>
        {
            try
            {
                var inStock = await service.IsInStockAsync(id);
                return Results.Ok(new { BookId = id, InStock = inStock });
            }
            catch (BookStore.Core.Exceptions.BookNotFoundException)
            {
                return Results.NotFound();
            }
        });

        books.MapGet("/{id:int}/discount", async (int id, decimal percentage, IBookService service) =>
        {
            try
            {
                var price = await service.CalculateDiscountPriceAsync(id, percentage);
                return Results.Ok(new { BookId = id, Percentage = percentage, DiscountedPrice = price });
            }
            catch (BookStore.Core.Exceptions.BookNotFoundException)
            {
                return Results.NotFound();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        books.MapPost("/", async (Book book, IBookService service) =>
        {
            try
            {
                var created = await service.AddBookAsync(book);
                return Results.Created($"/api/books/{created.Id}", created);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        books.MapPut("/{id:int}", async (int id, Book book, IBookService service) =>
        {
            if (id != book.Id)
                return Results.BadRequest("Route ID and body ID must match.");

            try
            {
                var updated = await service.UpdateBookAsync(book);
                return Results.Ok(updated);
            }
            catch (BookStore.Core.Exceptions.BookNotFoundException)
            {
                return Results.NotFound();
            }
        });

        books.MapDelete("/{id:int}", async (int id, IBookService service) =>
        {
            try
            {
                await service.RemoveBookAsync(id);
                return Results.NoContent();
            }
            catch (BookStore.Core.Exceptions.BookNotFoundException)
            {
                return Results.NotFound();
            }
        });
    }
}


