using BookStore.Core.Models;
using Xunit.Sdk;

namespace BookStore.UnitTests.TestData;

/// <summary>
/// Provides shared test-data collections used across multiple test classes.
/// Centralizing test data is a best practice that avoids duplication and
/// keeps individual test files focused on the test logic.
/// </summary>
public static class BookTestData
{
    // ── Sample books ─────────────────────────────────────────────────────────

    public static Book TechBook { get; } = new(
        Id: 1,
        Title: "The Pragmatic Programmer",
        Author: "David Thomas",
        Isbn: "978-0135957059",
        Price: 49.99m,
        Category: BookCategory.Technology,
        StockQuantity: 10,
        PublishedDate: new DateOnly(2019, 9, 13));

    public static Book ScienceBook { get; } = new(
        Id: 2,
        Title: "A Brief History of Time",
        Author: "Stephen Hawking",
        Isbn: "978-0553380163",
        Price: 14.99m,
        Category: BookCategory.Science,
        StockQuantity: 5,
        PublishedDate: new DateOnly(1998, 9, 1));

    public static Book OutOfStockBook { get; } = new(
        Id: 3,
        Title: "Rare Collector's Edition",
        Author: "Jane Doe",
        Isbn: "978-0000000001",
        Price: 199.99m,
        Category: BookCategory.Other,
        StockQuantity: 0,
        PublishedDate: new DateOnly(2020, 1, 1));

    /// <summary>
    /// A fixed catalog of books suitable for listing tests.
    /// </summary>
    public static IReadOnlyList<Book> AllBooks { get; } =
    [
        TechBook,
        ScienceBook,
        OutOfStockBook,
    ];

    // ── Theory data: discount calculations ───────────────────────────────────

    /// <summary>
    /// MemberData source for discount calculation theory tests.
    /// Each element: (bookId, originalPrice, discountPercentage, expectedPrice)
    /// </summary>
    public static TheoryData<int, decimal, decimal, decimal> DiscountTestCases =>
        new(
            (1, 100.00m, 10m, 90.00m), // 10% off $100 → $90
            (1, 100.00m, 25m, 75.00m), // 25% off $100 → $75
            (1, 50.00m,  50m, 25.00m),  // 50% off $50  → $25
            (1, 100.00m, 0m,  100.00m), // 0%  off → unchanged
            (1, 100.00m, 100m, 0.00m)); // 100% off → free


    // ── ClassData: invalid book definitions ──────────────────────────────────

    /// <summary>
    /// ClassData source that enumerates book definitions that must be rejected.
    /// </summary>
    public static TheoryData<Book> InvalidBookCases =>
        [
            // Empty title
            new Book(0, "", "Author", "978-0000000000", 9.99m, BookCategory.Fiction, 1, new DateOnly(2020, 1, 1)),
            // Whitespace-only title
            new Book(0, "   ", "Author", "978-0000000000", 9.99m, BookCategory.Fiction, 1, new DateOnly(2020, 1, 1)),
            // Negative price
            new Book(0, "Title", "Author", "978-0000000000", -1m, BookCategory.Fiction, 1, new DateOnly(2020, 1, 1)),
        ];
}

/// <summary>
/// Serializable wrapper for <see cref="Book"/> to enable xUnit v3 [ClassData]
/// enumeration in Test Explorer. This wrapper implements <see cref="IXunitSerializable"/>
/// to satisfy xUnit's serialization requirements without polluting the domain model.
/// </summary>
public sealed class SerializableBook : IXunitSerializable
{
    /// <summary>
    /// Parameterless constructor required by <see cref="IXunitSerializable"/>.
    /// </summary>
    public SerializableBook()
    {
        Book = new Book(0, string.Empty, string.Empty, string.Empty, 0m, BookCategory.Fiction, 0, default);
    }

    public SerializableBook(Book book)
    {
        Book = book;
    }

    public Book Book { get; private set; }

    public void Deserialize(IXunitSerializationInfo info)
    {
        Book = new Book(
            Id: info.GetValue<int>(nameof(Book.Id)),
            Title: info.GetValue<string>(nameof(Book.Title)) ?? string.Empty,
            Author: info.GetValue<string>(nameof(Book.Author)) ?? string.Empty,
            Isbn: info.GetValue<string>(nameof(Book.Isbn)) ?? string.Empty,
            Price: info.GetValue<decimal>(nameof(Book.Price)),
            Category: info.GetValue<BookCategory>(nameof(Book.Category)),
            StockQuantity: info.GetValue<int>(nameof(Book.StockQuantity)),
            PublishedDate: DateOnly.FromDayNumber(info.GetValue<int>(nameof(Book.PublishedDate))));
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(Book.Id), Book.Id);
        info.AddValue(nameof(Book.Title), Book.Title);
        info.AddValue(nameof(Book.Author), Book.Author);
        info.AddValue(nameof(Book.Isbn), Book.Isbn);
        info.AddValue(nameof(Book.Price), Book.Price);
        info.AddValue(nameof(Book.Category), Book.Category);
        info.AddValue(nameof(Book.StockQuantity), Book.StockQuantity);
        info.AddValue(nameof(Book.PublishedDate), Book.PublishedDate.DayNumber);
    }
}

/// <summary>
/// ClassData source for xUnit [ClassData] attribute demonstrating how to use
/// a serializable wrapper for complex domain objects. This approach keeps the
/// domain model (<see cref="Book"/>) clean of test framework dependencies while
/// still enabling Test Explorer to enumerate individual test cases.
/// </summary>
public sealed class InvalidBookClassData() : TheoryData<SerializableBook>([
    // Empty title
    new SerializableBook(new Book(0, "", "Author", "978-0000000000", 9.99m, BookCategory.Fiction, 1, new DateOnly(2020, 1, 1))),
    // Whitespace-only title
    new SerializableBook(new Book(0, "   ", "Author", "978-0000000000", 9.99m, BookCategory.Fiction, 1, new DateOnly(2020, 1, 1))),
    // Negative price
    new SerializableBook(new Book(0, "Valid Title", "Author", "978-0000000000", -1m, BookCategory.Fiction, 1, new DateOnly(2020, 1, 1)))
    ])
{ }
