using Microsoft.AspNetCore.Mvc;
using Order.Api.Data;

namespace Order.Api.Endpoints;

public class OrdersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/orders/{id:guid}", (Guid id) =>
        {
            var order = OrdersStore.GetById(id);

            return order is not null
                ? Results.Ok(order)
                : Results.NotFound(new { Message = $"Order {id} not found." });
        });

        app.MapPost("/api/orders", ([FromBody] Models.Order order) =>
        {
            OrdersStore.Save(order);

            return Results.Created($"/api/orders/{order.Id}", order);
        });


        app.MapGet("/api/orders/throw",() =>
        {
            throw new Exception("This is a simulated exception to test the error handling pipeline.");
        });
    }
}
