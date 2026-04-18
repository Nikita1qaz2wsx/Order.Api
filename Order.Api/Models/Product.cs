namespace Order.Api.Models;

public record Product(Guid Id, string Name, decimal ProductPrice, int Quantity)
{
    public decimal TotalPrice => ProductPrice * Quantity;
}