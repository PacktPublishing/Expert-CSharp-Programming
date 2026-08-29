// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

namespace AppStartup;

public record Order(int Id, string CustomerName, decimal Total, DateTime PlacedAt);

public interface IOrderRepository
{
    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Order> AddAsync(Order order, CancellationToken ct = default);
}

/// <summary>
/// In-memory stand-in that simulates what a real SQL implementation would do.
/// Replace the body with EF Core / Dapper calls against a real database.
/// </summary>
public class SqlOrderRepository : IOrderRepository
{
    // Simulates a database table; seeded with a couple of sample rows.
    private readonly List<Order> _store =
    [
        new(1, "Alice Smith",  149.99m, new DateTime(2025, 1, 10, 9,  0, 0, DateTimeKind.Utc)),
        new(2, "Bob Johnson",   79.50m, new DateTime(2025, 3, 22, 14, 30, 0, DateTimeKind.Utc)),
    ];

    public Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Order>>(_store.AsReadOnly());

    public Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_store.FirstOrDefault(o => o.Id == id));

    public Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        int nextId = _store.Count == 0 ? 1 : _store.Max(o => o.Id) + 1;
        Order saved = order with { Id = nextId };
        _store.Add(saved);
        return Task.FromResult(saved);
    }
}
