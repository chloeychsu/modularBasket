﻿
using Mapster;

namespace Ordering.Orders.Features.GetOrderById;

public record GetOrderByIdQuery(Guid Id):IQuery<GetOrderByIdResult>;
public record GetOrderByIdResult(OrderDto Order);
public class GetOrderByIdHandler(OrderingDbContext context) : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
{
    public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await context.Orders.AsNoTracking().Include(x=>x.Items).SingleOrDefaultAsync(p=>p.Id==query.Id,cancellationToken);

        if(order is null){
            throw new OrderNotFoundException(query.Id);
        }
        var orderDto = order.Adapt<OrderDto>();

        return new GetOrderByIdResult(orderDto);
    }
}
