using Microsoft.AspNetCore.Http.HttpResults;
using Books.Services;
using Books.Data;
using Books.API.Endpoints;
using System.Diagnostics;
using Books.API.Infrastructure;
namespace BooksService.Endpoints;

public static class BookEndpoints 
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/books")
            .AddEndpointFilter<BookServiceExceptionFilter>()
            .WithTags(nameof(Book));

        group.MapGet("/", async Task<Results<Ok<IEnumerable<Book>>, InternalServerError>> (IBooksService booksService, [FromKeyedServices("BooksAPIActivity")] ActivitySource activitySource, CancellationToken cancellationToken, HttpRequest request) =>
        {
            KeyValuePair<string, object?> k1 = new("mytag1", "myvalue1");
            KeyValuePair<string, object?> k2 = new("mytag2", "myvalue2");

            using var activity = activitySource.CreateActivity("Books.GetAll", ActivityKind.Server);
            activity?.SetTag("mytag1", "myvalue1");
            activity?.SetTag("http.method", request.Method);
            activity?.SetTag("http.url", request.Path);
            activity?.SetTag("http.scheme", request.Scheme);
            activity?.SetTag("http.host", request.Host.Host);
            activity?.Start();

            var books = await booksService.GetBooksAsync(cancellationToken);
            activity?.AddEvent(new ActivityEvent("RetrievedBooks"));
            if (activity is not null)
            {
                activity.SetTag("books.count", books.Count());
                activity.SetStatus(ActivityStatusCode.Ok);
            }
            return TypedResults.Ok(books);
        })
        .WithName("GetAllBooks")
        .AddOpenApiOperationTransformer((operation, _, _) =>
        {
            operation.Summary = "Gets all books";
            operation.Description = "Gets all books from the database.";
            return Task.CompletedTask;
        });

        group.MapGet("/{id}", async Task<Results<Ok<Book>, NotFound, InternalServerError>> (int id, IBooksService booksService, [FromKeyedServices("BooksAPIActivity")] ActivitySource activitySource, CancellationToken cancellationToken) =>
        {
            using var activity = activitySource.StartActivity("Books.GetById", ActivityKind.Server, parentContext: default);
            activity?.SetTag("book.id", id);
            var book = await booksService.GetBookByIdAsync(id, cancellationToken);
            if (book is null)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "Book not found");
                return TypedResults.NotFound();
            }
            activity?.SetStatus(ActivityStatusCode.Ok);
            return TypedResults.Ok(book);
        })
        .WithName("GetBookById")
        .AddOpenApiOperationTransformer((operation, _, _) =>
        {
            operation.Summary = "Gets a book by ID";
            operation.Description = "Gets a single book from the database by its ID.";
            return Task.CompletedTask;
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest, InternalServerError>> (int id, Book book, IBooksService booksService, [FromKeyedServices("BooksAPIActivity")] ActivitySource activitySource, CancellationToken cancellationToken) =>
        {
            using var activity = activitySource.StartActivity("Books.Update", ActivityKind.Server);
            activity?.SetTag("book.id", id);
            var affected = await booksService.UpdateBookAsync(book, cancellationToken);
            if (affected == 1)
            {
                activity?.SetStatus(ActivityStatusCode.Ok);
                return TypedResults.Ok();
            }
            activity?.SetStatus(ActivityStatusCode.Error, "Book not found");
            return TypedResults.NotFound();
        })
        .AddEndpointFilter<ValidateMatchIdEndpointFilter>()
        .WithName("UpdateBook")
        .AddOpenApiOperationTransformer((operation, _, _) =>
        {
            operation.Summary = "Updates a book";
            operation.Description = "Updates a book in the database.";
            return Task.CompletedTask;
        });

        group.MapPost("/", async Task<Results<Created<Book>, InternalServerError>> (Book book, IBooksService booksService, [FromKeyedServices("BooksAPIActivity")] ActivitySource activitySource, BooksAPIMetrics metrics, CancellationToken cancellationToken) =>
        {
            using var activity = activitySource.CreateActivity("Books.Create", ActivityKind.Server);
            activity?.SetTag("book.title", book.Title);
            activity?.Start();
            var created = await booksService.CreateBookAsync(book,cancellationToken);
            metrics.BookCreated();
            activity?.SetTag("book.id", created.Id);
            activity?.SetStatus(ActivityStatusCode.Ok);
            return TypedResults.Created($"/api/Book/{created.Id}", created);
        })
        .WithName("CreateBook")
        .AddOpenApiOperationTransformer((operation, _, _) =>
        {
            operation.Summary = "Creates a new book";
            operation.Description = "Creates a new book in the database.";
            return Task.CompletedTask;
        });

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound, InternalServerError>> (int id, IBooksService booksService, [FromKeyedServices("BooksAPIActivity")] ActivitySource activitySource, CancellationToken cancellationToken) =>
        {
            using var activity = activitySource.StartActivity("Books.Delete", ActivityKind.Server);
            activity?.SetTag("book.id", id);
            var affected = await booksService.DeleteBookAsync(id, cancellationToken);
            if (affected == 1)
            {
                activity?.SetStatus(ActivityStatusCode.Ok);
                return TypedResults.Ok();
            }
            activity?.SetStatus(ActivityStatusCode.Error, "Book not found");
            return TypedResults.NotFound();
        })
        .WithName("DeleteBook")
        .AddOpenApiOperationTransformer((operation, _, _) =>
        {
            operation.Summary = "Deletes a book";
            operation.Description = "Deletes a book from the database.";
            return Task.CompletedTask;
        });
    }
}
