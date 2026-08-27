using System.Diagnostics.Metrics;

namespace Books.API.Infrastructure;

public sealed class BooksAPIMetrics : IDisposable
{
    private readonly Meter _meter;
    private readonly Counter<long> _booksCreatedCounter;
    private readonly Counter<long> _numberQueriesCounter;

    public BooksAPIMetrics(IMeterFactory meterFactory)
    {
        _meter = meterFactory.Create("BooksAPI");

        _booksCreatedCounter = _meter.CreateCounter<long>(
            name: "booksapi.books.created",
            unit: "count",
            description: "Number of books created");

        _numberQueriesCounter = _meter.CreateCounter<long>(
            name: "booksapi.books.queries",
            unit: "count",
            description: "Number of book queries executed");
    }

    public void BookCreated() => 
        _booksCreatedCounter.Add(1);

    public void BookQueryExecuted(long count = 1)
    {
        if (count <= 0) return;
        _numberQueriesCounter.Add(count);
    }

    public void Dispose() => _meter.Dispose();
}
