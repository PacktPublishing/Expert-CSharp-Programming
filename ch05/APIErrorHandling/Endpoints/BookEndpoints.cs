using APIErrorHandling.Data;
using APIErrorHandling.Models;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace APIErrorHandling.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Book")
            .WithTags(nameof(Book));

        group.MapGet("/", async (BooksContext db) =>
        {
            return await db.Book.ToListAsync();
        })
        .WithName("GetAllBooks")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Book>, NotFound>> (int id, BooksContext db) =>
        {
            return await db.Book.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Book model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBookById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Book book, BooksContext db) =>
        {
            var affected = await db.Book
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, book.Id)
                    .SetProperty(m => m.Title, book.Title)
                    .SetProperty(m => m.Publisher, book.Publisher)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBook")
        .WithOpenApi();

        group.MapPost("/", async (Book book, BooksContext db) =>
        {
            db.Book.Add(book);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Book/{book.Id}", book);
        })
        .WithName("CreateBook")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, BooksContext db) =>
        {
            var affected = await db.Book
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBook")
        .WithOpenApi();
    }
}
