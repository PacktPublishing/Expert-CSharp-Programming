using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

using JsonSerialization.Converters;
using JsonSerialization.Data;
using JsonSerialization.Models;

namespace JsonSerialization;

internal static class Runner
{
    private static readonly Library _sampleLibrary = new(
        Name: "City Central Library",
        Location: "Downtown, Floor 2",
        Books:
        [
            new(1, "Pragmatic Microservices",  "Christian Nagel", new  Money(51.99m, "USD"), new DateOnly(2024, 5, 31), ["dotnet", "aspire", "microservices"]),
            new(2, "C# in Depth", "Jon Skeet", new Money(46.99m, "USD"), new DateOnly(2019, 3, 23), ["csharp", "programming"]),
            new(3, "Effective .NET Memory Management", "Trevoir Williams", new Money(43.99m, "USD"), new DateOnly(2024, 7, 30), ["dotnet", "memory"]),
            new(4, "Clean Code", "Robert C. Martin", new Money(59.99m, "USD"), new DateOnly(2025, 10, 18),["best-practices"]),
            new(5, "Data Science with .NET", "Martin Kleppmann",
                new Money(49.99m, "USD"), new DateOnly(2024, 10, 30), ["databases", "distributed"]),
        ]);

    private static readonly JsonSerializerOptions _reflectionOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new MoneyJsonConverter() },
    };

    public static void BasicSerialization()
    {
        Console.WriteLine("Basic Serialization (reflection-based)");
        Console.WriteLine("--------------------------------------");

        Book singleBook = _sampleLibrary.Books[0];
        string json = JsonSerializer.Serialize(singleBook, _reflectionOptions);
        Console.WriteLine(json);
        Console.WriteLine();

        Book? deserialized = JsonSerializer.Deserialize<Book>(json, _reflectionOptions);
        Console.WriteLine($"Deserialized: {deserialized?.DisplayTitle}  –  {deserialized?.Price}");
        Console.WriteLine();
    }

    public static void HierarchicalGraph()
    {
        Console.WriteLine("Hierarchical Object Graph");
        Console.WriteLine("-------------------------");

        string libraryJson = JsonSerializer.Serialize(_sampleLibrary, _reflectionOptions);
        Console.WriteLine(libraryJson[..Math.Min(libraryJson.Length, 400)] + "...");
        Console.WriteLine();

        Library? libraryBack = JsonSerializer.Deserialize<Library>(libraryJson, _reflectionOptions);
        Console.WriteLine($"Deserialized library: {libraryBack?.Name} — {libraryBack?.TotalBooks} books");
        Console.WriteLine();
    }

    public static void SourceGeneratorSerialization()
    {
        Console.WriteLine("Source-Generator Serialization (compile-time, AOT-safe)");
        Console.WriteLine("-------------------------------------------------------");

        // Serialize using the compile-time generated context — no runtime reflection required
        string sgJson = JsonSerializer.Serialize(_sampleLibrary, LibraryJsonContext.Default.Library);
        Console.WriteLine($"Serialized {_sampleLibrary.TotalBooks} books via source generator.");

        Library? sgLibrary = JsonSerializer.Deserialize(sgJson, LibraryJsonContext.Default.Library);
        Console.WriteLine($"Deserialized: {sgLibrary?.Name} — {sgLibrary?.TotalBooks} books");
        Console.WriteLine();
    }

    public static void GenericPagedResultSerialization()
    {
        Console.WriteLine("Generic PagedResult<Book> Collection");
        Console.WriteLine("------------------------------------");

        PagedResult<Book> page = new(
            Items: [.. _sampleLibrary.Books.Take(2)],
            PageNumber: 1,
            PageSize: 2,
            TotalCount: _sampleLibrary.TotalBooks);

        string pageJson = JsonSerializer.Serialize(page, LibraryJsonContext.Default.PagedResultBook);
        Console.WriteLine(pageJson);
        Console.WriteLine();
    }

    public static async Task AsyncStreamingSerializationAsync()
    {
        Console.WriteLine("Async Streaming Serialization");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("Streaming books from a MemoryStream asynchronously...");

        using MemoryStream stream = new();
        await JsonSerializer.SerializeAsync(stream, [.. _sampleLibrary.Books], LibraryJsonContext.Default.ListBook);

        stream.Position = 0;
        List<Book>? streamedBooks = await JsonSerializer.DeserializeAsync(stream, LibraryJsonContext.Default.ListBook);
        Console.WriteLine($"Streamed and deserialized {streamedBooks?.Count} books.");
        Console.WriteLine();
    }

    public static void PerformanceComparison()
    {
        Console.WriteLine("Performance Comparison: Reflection vs Source Generator");
        Console.WriteLine("------------------------------------------------------");

        const int Iterations = 50_000;
        Book singleBook = _sampleLibrary.Books[0];

        // Warm-up
        for (int i = 0; i < 100; i++)
        {
            _ = JsonSerializer.Serialize(singleBook, _reflectionOptions);
            _ = JsonSerializer.Serialize(singleBook, LibraryJsonContext.Default.Book);
        }

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < Iterations; i++)
            _ = JsonSerializer.Serialize(singleBook, _reflectionOptions);
        sw.Stop();
        long reflectionMs = sw.ElapsedMilliseconds;

        sw.Restart();
        for (int i = 0; i < Iterations; i++)
            _ = JsonSerializer.Serialize(singleBook, LibraryJsonContext.Default.Book);
        sw.Stop();
        long sourceGenMs = sw.ElapsedMilliseconds;

        Console.WriteLine($"{Iterations:N0} serializations:");
        Console.WriteLine($"Reflection:      {reflectionMs,6} ms");
        Console.WriteLine($"Source generator:{sourceGenMs,6} ms");
        Console.WriteLine($"Speedup:         {(double)reflectionMs / Math.Max(sourceGenMs, 1):0.0}×");
        Console.WriteLine();
    }

    public static void JsonConverterErrorHandling()
    {
        Console.WriteLine("Custom Converter + Error Handling");
        Console.WriteLine("---------------------------------");

        string malformedMoneyJson = """{"id":1,"title":"Test","author":"A","price":"bad format","publishedOn":"2024-01-01","tags":[]}""";

        try
        {
            _ = JsonSerializer.Deserialize<Book>(malformedMoneyJson, _reflectionOptions);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Caught expected JsonException: {ex.Message[..Math.Min(ex.Message.Length, 80)]}...");
        }
        Console.WriteLine();
    }
}
