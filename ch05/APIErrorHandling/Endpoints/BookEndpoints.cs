using APIErrorHandling.Data;
using APIErrorHandling.Models;

using Microsoft.AspNetCore.Http.HttpResults;
namespace APIErrorHandling.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Books")
            .WithTags(nameof(Book));

        group.MapGet("/", async (IBooksService booksService) =>
        {
            return await booksService.GetBooksAsync();
        })
        .WithName("GetAllBooks")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Book>, NotFound>> (int id, IBooksService booksService) =>
        {
            return await booksService.GetBookByIdAsync(id)
                is Book model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBookById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (int id, Book book, IBooksService booksService) =>
        {
            if (id != book.Id)
            {
                return TypedResults.BadRequest();
            }

            var affected = await booksService.UpdateBookAsync(book);

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBook")
        .WithOpenApi();

        group.MapPost("/", async (Book book, IBooksService booksService) =>
        {
            book = await booksService.CreateBookAsync(book);
            return TypedResults.Created($"/api/Book/{book.Id}", book);
        })
        .WithName("CreateBook")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, IBooksService booksService) =>
        {
            var affected = await booksService.DeleteBookAsync(id);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBook")
        .WithOpenApi();
    }
}
