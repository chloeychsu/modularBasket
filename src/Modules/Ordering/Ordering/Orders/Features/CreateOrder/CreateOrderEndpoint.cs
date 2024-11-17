using Carter;
using Microsoft.AspNetCore.Routing;

namespace Ordering.Orders.Features.CreateOrder;

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);
public class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
    }
}
