using System.Net.NetworkInformation;

namespace GRPCService.Services;

// <summary>
/// Server-side implementation of the <see cref="BookCatalog.BookCatalogBase"/> gRPC service.
/// Demonstrates all four RPC streaming variants.
/// </summary>
public sealed class BookCatalogService : BookCatalogBase
{
    // In-memory catalog seeded at startup.
    // All mutations are guarded by _lock to handle concurrent gRPC calls.
    private static readonly Lock _lock = new();
    private static readonly List<BookMessage> s_catalog =
    [
        new() { Id = 1, Title = "C# in Depth",        Author = "Jon Skeet",          Price = "39.99 USD", PublishedOn = "2019-03-01", Tags = { "csharp", "programming" } },
        new() { Id = 2, Title = "Pro .NET Memory",     Author = "Konrad Kokosa",      Price = "44.99 USD", PublishedOn = "2020-05-15", Tags = { "dotnet", "memory", "performance" } },
        new() { Id = 3, Title = "Expert C#",           Author = "Christian Nagel",    Price = "49.99 USD", PublishedOn = "2024-01-01", Tags = { "csharp", "expert" } },
        new() { Id = 4, Title = "Clean Code",          Author = "Robert C. Martin",   Price = "34.99 USD", PublishedOn = "2008-08-01", Tags = { "best-practices" } },
        new() { Id = 5, Title = "Designing Data-Intensive Applications", Author = "Martin Kleppmann",
                Price = "54.99 USD", PublishedOn = "2017-03-16", Tags = { "databases", "distributed" } },
    ];

    // ----------------------------------------------------------------
    // 1. Unary RPC
    // ----------------------------------------------------------------

    /// <summary>Returns a single book identified by <paramref name="request"/>.Id.</summary>
    public override Task<BookMessage> GetBook(GetBookRequest request, ServerCallContext context)
    {
        BookMessage? book;
        lock (_lock)
            book = s_catalog.Find(b => b.Id == request.Id);

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
                ? [.. s_catalog]
                : [.. s_catalog.Where(b => b.Tags.Contains(request.TagFilter))];
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
                book.Id = s_catalog.Count + 1;
                s_catalog.Add(book);
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
                duplicate = s_catalog.Exists(b => b.Title == req.Book.Title && b.Author == req.Book.Author);
                if (!duplicate)
                {
                    BookMessage copy = req.Book.Clone();
                    copy.Id = s_catalog.Count + 1;
                    s_catalog.Add(copy);
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
