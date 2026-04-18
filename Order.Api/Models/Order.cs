namespace Order.Api.Models;

public record Order(Guid Id, List<Product> OrderItems)
{
    public decimal TotalPrice => OrderItems.Sum(p => p.TotalPrice);
}