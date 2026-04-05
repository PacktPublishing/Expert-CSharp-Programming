using BookStore.Core.Models;

namespace BookStore.TUnitTests;

/// <summary>
/// Shared sample book definitions used across the TUnit test suite.
/// Using a static class avoids duplication while keeping test data close
/// to the test project (separate from the xUnit data in BookStore.UnitTests).
/// </summary>
internal static class SampleBooks
{
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

    public static IReadOnlyList<Book> All { get; } = [TechBook, ScienceBook, OutOfStockBook];
}
