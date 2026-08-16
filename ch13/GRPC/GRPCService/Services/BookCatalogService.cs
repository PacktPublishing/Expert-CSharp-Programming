// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Grpc.Core;

namespace GRPCService.Services;

// <summary>
/// Server-side implementation of the <see cref="BookCatalog.BookCatalogBase"/> gRPC service.
/// Demonstrates all four RPC streaming variants.
/// </summary>
public sealed class BookCatalogService : GRPCService.BookCatalog.BookCatalogBase
{
    // In-memory catalog seeded at startup.
    // All mutations are guarded by _lock to handle concurrent gRPC calls.
    private static readonly Lock _lock = new();

    private static readonly List<BookMessage> _catalog =
    [
        new() { Id = 1, Title = "Pragmatic Microservices", Author = "Christian Nagel", Price = "51.99 USD", PublishedOn = "2024-05-31", Tags = { "dotnet", "aspire", "microservices" } },
        new() { Id = 2, Title = "C# in Depth", Author = "Jon Skeet", Price = "46.99 USD", PublishedOn = "2019-03-23", Tags = { "csharp", "programming" } },
        new() { Id = 3, Title = "Effective .NET Memory Management", Author = "Trevoir Williams", Price = "43.99 USD", PublishedOn = "2024-07-30", Tags = { "dotnet", "memory" } },
        new() { Id = 4, Title = "Clean Code", Author = "Robert C. Martin", Price = "59.99 USD", PublishedOn = "2025-10-18", Tags = { "best-practices" } },
        new() { Id = 5, Title = "Data Science with .NET", Author = "Martin Kleppmann",
            Price = "49.99 USD", PublishedOn = "2024-10-30", Tags = { "databases", "distributed" } },
    ];

    // ----------------------------------------------------------------
    // 1. Unary RPC
    // ----------------------------------------------------------------

    /// <summary>Returns a single book identified by <paramref name="request"/>.Id.</summary>
    public override Task<BookMessage> GetBook(GetBookRequest request, ServerCallContext context)
    {
        BookMessage? book;
        lock (_lock)
            book = _catalog.Find(b => b.Id == request.Id);

        if (book is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Book {request.Id} not found."));

        return Task.FromResult(book);
    }

    // ----------------------------------------------------------------
    // 2. Server-streaming RPC
    // ----------------------------------------------------------------

    /// <summary>
    /// Streams all books whose tags contain <see cref="ListBooksRequest.TagFilter"/>
    /// (or all books if the filter is empty).
    /// </summary>
    public override async Task ListBooks(
        ListBooksRequest request,
        IServerStreamWriter<BookMessage> responseStream,
        ServerCallContext context)
    {
        // Snapshot the catalog under lock to avoid mutation during enumeration
        List<BookMessage> snapshot;
        lock (_lock)
        {
            snapshot = string.IsNullOrWhiteSpace(request.TagFilter)
                ? [.. _catalog]
                : [.. _catalog.Where(b => b.Tags.Contains(request.TagFilter))];
        }

        foreach (BookMessage book in snapshot)
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            await responseStream.WriteAsync(book);
            // Simulate realistic inter-message latency
            await Task.Delay(20, context.CancellationToken);
        }
    }

    // ----------------------------------------------------------------
    // 3. Client-streaming RPC
    // ----------------------------------------------------------------

    /// <summary>Accepts a stream of books to add to the catalog and returns the total count.</summary>
    public override async Task<AddBooksResponse> AddBooks(
        IAsyncStreamReader<AddBooksRequest> requestStream,
        ServerCallContext context)
    {
        int added = 0;
        await foreach (AddBooksRequest req in requestStream.ReadAllAsync(context.CancellationToken))
        {
            BookMessage book = req.Book;
            lock (_lock)
            {
                book.Id = _catalog.Count + 1;
                _catalog.Add(book);
            }
            added++;
        }
        return new AddBooksResponse { BooksAdded = added };
    }

    // ----------------------------------------------------------------
    // 4. Bidirectional streaming RPC
    // ----------------------------------------------------------------

    /// <summary>
    /// Receives a stream of books to sync, and writes back a per-book status.
    /// Demonstrates fully asynchronous bidirectional streaming.
    /// </summary>
    public override async Task SyncBooks(
        IAsyncStreamReader<SyncBookRequest> requestStream,
        IServerStreamWriter<SyncBookResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (SyncBookRequest req in requestStream.ReadAllAsync(context.CancellationToken))
        {
            bool duplicate;
            lock (_lock)
            {
                duplicate = _catalog.Exists(b => b.Title == req.Book.Title && b.Author == req.Book.Author);
                if (!duplicate)
                {
                    BookMessage copy = req.Book.Clone();
                    copy.Id = _catalog.Count + 1;
                    _catalog.Add(copy);
                }
            }

            string status = duplicate ? "DUPLICATE" : "OK";
            await responseStream.WriteAsync(new SyncBookResponse
            {
                Id = req.Book.Id,
                Status = status,
            });
        }
    }
}
