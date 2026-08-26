namespace SignalR.Blazor.Client.Models;

public sealed record class StockTick(string Symbol, decimal Price, DateTimeOffset Timestamp);
