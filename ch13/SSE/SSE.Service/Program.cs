// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Globalization;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/stocks/{symbol}", (string symbol, CancellationToken ct) =>
    TypedResults.ServerSentEvents(StreamPrices(symbol, ct)));

app.Run();

static async IAsyncEnumerable<SseItem<StockTick>> StreamPrices(
    string symbol,
    [EnumeratorCancellation] CancellationToken ct = default)
{
    decimal price = 100m;

    while (!ct.IsCancellationRequested)
    {
        DateTimeOffset timestamp = DateTimeOffset.UtcNow;
        price += (decimal)(Random.Shared.NextDouble() * 2 - 1);

        yield return new SseItem<StockTick>(
            new StockTick(symbol, "price", Math.Max(price, 1m), timestamp, timestamp.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture)));

        await Task.Delay(1_000, ct);
    }
}

sealed record class StockTick(string Symbol, string EventType, decimal Price, DateTimeOffset Timestamp, string EventId);
