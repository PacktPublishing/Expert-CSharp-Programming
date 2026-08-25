// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.SignalR;

namespace SignalR.Service;

public sealed class MarketHub : Hub
{
    public async IAsyncEnumerable<StockTick> StreamPrices(
        string symbol,
        [EnumeratorCancellation] CancellationToken ct)
    {
        Random rng = new(42);
        decimal price = 100m;

        while (!ct.IsCancellationRequested)
        {
            price += (decimal)(rng.NextDouble() * 2 - 1);
            yield return new StockTick(symbol, Math.Max(price, 1m), DateTimeOffset.UtcNow);
            await Task.Delay(500, ct);
        }
    }
}

public sealed record class StockTick(string Symbol, decimal Price, DateTimeOffset Timestamp);
