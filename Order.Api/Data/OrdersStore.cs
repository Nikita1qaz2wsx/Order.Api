using System.Collections.Concurrent;

namespace Order.Api.Data;

public static class OrdersStore
{
    private static readonly ConcurrentDictionary<Guid, Models.Order> _orders = new();

    // Adds or updates an order
    public static void Save(Models.Order order) => _orders[order.Id] = order;

    // Retrieves a single order by ID
    public static Models.Order? GetById(Guid id) => _orders.TryGetValue(id, out var order) ? order : null;

    // Retrieves all stored orders
    public static IEnumerable<Models.Order> GetAll() => _orders.Values;

    // Removes an order
    public static bool Delete(Guid id) => _orders.TryRemove(id, out _);
}