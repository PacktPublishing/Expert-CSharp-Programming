// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Grpc.Core;

using GRPCService;

namespace GRPCClient;

internal class Runner(BookCatalog.BookCatalogClient client)
{
    public async Task CallUnaryAsync()
    {
        BookMessage book = await client.GetBookAsync(new GetBookRequest { Id = 1 });

        Console.WriteLine();
        Console.WriteLine("Unary RPC - GetBook");
        PrintBook(book);
    }

    public async Task CallServerStreamingAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Server streaming RPC - ListBooks(tag: dotnet)");

        using var call = client.ListBooks(new ListBooksRequest { TagFilter = "dotnet" });

        await foreach (BookMessage book in call.ResponseStream.ReadAllAsync())
        {
            PrintBook(book);
        }
    }

    public async Task CallClientStreamingAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Client streaming RPC - AddBooks");

        using var call = client.AddBooks();

        foreach (BookMessage book in GetBooksToAdd())
        {
            await call.RequestStream.WriteAsync(new AddBooksRequest { Book = book });
            Console.WriteLine($"Queued: {book.Title}");
        }

        await call.RequestStream.CompleteAsync();

        AddBooksResponse response = await call.ResponseAsync;
        Console.WriteLine($"Books added: {response.BooksAdded}");
    }

    public async Task CallBidirectionalStreamingAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Bidirectional streaming RPC - SyncBooks");

        using var call = client.SyncBooks();

        Task readResponsesTask = ReadSyncResponsesAsync(call.ResponseStream);

        foreach (BookMessage book in GetBooksToSync())
        {
            await call.RequestStream.WriteAsync(new SyncBookRequest { Book = book });
            Console.WriteLine($"Sent: {book.Title}");
        }

        await call.RequestStream.CompleteAsync();
        await readResponsesTask;
    }

    private static async Task ReadSyncResponsesAsync(IAsyncStreamReader<SyncBookResponse> responseStream)
    {
        await foreach (SyncBookResponse response in responseStream.ReadAllAsync())
        {
            Console.WriteLine($"Book {response.Id}: {response.Status}");
        }
    }

    private static IEnumerable<BookMessage> GetBooksToAdd() =>
    [
        new()
        {
            Title = "Applied gRPC for .NET",
            Author = "Christian Nagel",
            Price = "39.99 USD",
            PublishedOn = "2026-01-10",
            Tags = { "dotnet", "grpc", "distributed" }
        },
        new()
        {
            Title = "Minimal APIs in Practice",
            Author = "Jane Doe",
            Price = "34.99 USD",
            PublishedOn = "2025-11-02",
            Tags = { "aspnetcore", "web", "csharp" }
        }
    ];

    private static IEnumerable<BookMessage> GetBooksToSync() =>
    [
        new()
        {
            Id = 100,
            Title = "Pragmatic Microservices",
            Author = "Christian Nagel",
            Price = "51.99 USD",
            PublishedOn = "2024-05-31",
            Tags = { "dotnet", "aspire", "microservices" }
        },
        new()
        {
            Id = 101,
            Title = "Streaming with gRPC",
            Author = "John Smith",
            Price = "42.50 USD",
            PublishedOn = "2026-02-18",
            Tags = { "grpc", "streaming", "distributed" }
        }
    ];

    private static void PrintBook(BookMessage book)
    {
        string tags = book.Tags.Count > 0 ? string.Join(", ", book.Tags) : "-";
        Console.WriteLine($"[{book.Id}] {book.Title} by {book.Author} | {book.Price} | {book.PublishedOn} | {tags}");
    }
}
