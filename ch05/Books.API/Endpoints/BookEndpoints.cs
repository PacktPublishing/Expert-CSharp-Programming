using Microsoft.AspNetCore.Http.HttpResults;
using Books.Services;
using Books.Data;
using Books.API.Endpoints;
namespace BooksService.Endpoints;

public static class BookEndpoints 
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/books")
            .AddEndpointFilter<BookServiceExceptionFilter>()
            .WithTags(nameof(Book));

        group.MapGet("/", async Task<Results<Ok<IEnumerable<Book>>, InternalServerError>> (IBooksService booksService, CancellationToken cancellationToken) =>
        {
            var books = await booksService.GetBooksAsync(cancellationToken);
            return TypedResults.Ok(books);
        })
        .WithName("GetAllBooks")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Book>, NotFound, InternalServerError>> (int id, IBooksService booksService, CancellationToken cancellationToken) =>
        {
            return await booksService.GetBookByIdAsync(id, cancellationToken)
                is Book model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBookById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest, InternalServerError>> (int id, Book book, IBooksService booksService, CancellationToken cancellationToken) =>
        {
            //if (id != book.Id)
            //{
            //    return TypedResults.BadRequest();
            //}

            var affected = await booksService.UpdateBookAsync(book, cancellationToken);

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .AddEndpointFilter<ValidateMatchIdEndpointFilter>()
        .WithName("UpdateBook")
        .WithOpenApi();

        group.MapPost("/", async Task<Results<Created<Book>, InternalServerError>> (Book book, IBooksService booksService, CancellationToken cancellationToken) =>
        {
            book = await booksService.CreateBookAsync(book,cancellationToken);
            return TypedResults.Created($"/api/Book/{book.Id}", book);
        })
        .WithName("CreateBook")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound, InternalServerError>> (int id, IBooksService booksService, CancellationToken cancellationToken) =>
        {
            var affected = await booksService.DeleteBookAsync(id, cancellationToken);
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBook")
        .WithOpenApi();
    }
}
