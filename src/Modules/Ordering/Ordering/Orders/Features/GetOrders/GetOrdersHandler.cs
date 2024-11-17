using Shared.Pagination;

namespace Ordering.Orders.Features.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest) : IQuery<GetOrdersResult>;

public record GetOrdersResult(PaginationResult<OrderDto> Orders);
public class GetOrdersHandler(OrderingDbContext context) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.pageIndex;
        var pageSize = query.PaginationRequest.pageSize;

        var totalCount = await context.Orders.LongCountAsync(cancellationToken);

        var orders = await context.Orders
                        .AsNoTracking()
                        .Include(x => x.Items)
                        .OrderBy(x => x.OrderName)
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .ToListAsync(cancellationToken);
        
        var orderDtos = orders.Adapt<List<OrderDto>>();

        return new GetOrdersResult(new PaginationResult<OrderDto>(pageIndex,pageSize,totalCount,orderDtos));

    }
}