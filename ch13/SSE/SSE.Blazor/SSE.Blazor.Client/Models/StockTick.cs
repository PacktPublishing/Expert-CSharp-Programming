// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

namespace SSE.Blazor.Client.Models;

/// <summary>Matches the StockTick record produced by SSE.Service.</summary>
sealed record StockTick(
    string Symbol,
    string EventType,
    decimal Price,
    DateTimeOffset Timestamp,
    string EventId);
