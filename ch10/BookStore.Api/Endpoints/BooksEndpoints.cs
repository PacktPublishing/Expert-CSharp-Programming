using BookStore.Core.Exceptions;
using BookStore.Core.Models;
using BookStore.Core.Services;

namespace BookStore.Api.Endpoints;

/// <summary>
/// Defines all HTTP endpoints for the Books API.
/// Maps routes under /api/books and handles CRUD operations.
/// </summary>
public static class BooksEndpoints
{
    /// <summary>
    /// Maps all book-related endpoints to the application.
    /// </summary>
    /// <param name="app">The web application to add endpoints to.</param>
    /// <returns>The route group builder for further configuration.</returns>
    public static RouteGroupBuilder MapBooksEndpoints(this IEndpointRouteBuilder app)
    {
        var books = app.MapGroup("/api/books").WithTags("Books");

        books.MapGet("/", async (IBookService service) =>
            Results.Ok(await service.GetAllBooksAsync()))
            .WithSummary("Returns all books in the catalog.");

        books.MapGet("/{id:int}", async (int id, IBookService service) =>
        {
            var book = await service.GetBookAsync(id);
            return book is null ? Results.NotFound() : Results.Ok(book);
        })
        .WithSummary("Returns a single book by ID.");

        books.MapGet("/category/{category}", async (BookCategory category, IBookService service) =>
            Results.Ok(await service.GetBooksByCategoryAsync(category)))
            .WithSummary("Returns books filtered by category.");

        books.MapGet("/search", async (string? q, IBookService service) =>
            Results.Ok(await service.SearchBooksAsync(q ?? string.Empty)))
            .WithSummary("Searches books by title, author, or ISBN.");

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
        })
        .WithSummary("Adds a new book to the catalog.");

        books.MapPut("/{id:int}", async (int id, Book book, IBookService service) =>
        {
            if (id != book.Id)
                return Results.BadRequest("Route ID and body ID must match.");

            try
            {
                var updated = await service.UpdateBookAsync(book);
                return Results.Ok(updated);
            }
            catch (BookNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithSummary("Updates an existing book.");

        books.MapDelete("/{id:int}", async (int id, IBookService service) =>
        {
            try
            {
                await service.RemoveBookAsync(id);
                return Results.NoContent();
            }
            catch (BookNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithSummary("Removes a book from the catalog.");

        books.MapGet("/{id:int}/discount", async (int id, decimal percentage, IBookService service) =>
        {
            try
            {
                var price = await service.CalculateDiscountPriceAsync(id, percentage);
                return Results.Ok(new { BookId = id, Percentage = percentage, DiscountedPrice = price });
            }
            catch (BookNotFoundException)
            {
                return Results.NotFound();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithSummary("Calculates the discounted price for a book.");

        books.MapGet("/{id:int}/stock", async (int id, IBookService service) =>
        {
            try
            {
                var inStock = await service.IsInStockAsync(id);
                return Results.Ok(new { BookId = id, InStock = inStock });
            }
            catch (BookNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithSummary("Checks whether a book is currently in stock.");

        return books;
    }
}
