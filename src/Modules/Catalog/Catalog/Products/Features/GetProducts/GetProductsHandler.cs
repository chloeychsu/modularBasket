namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest paginationRequest) : IQuery<GetProductResult>;

public record GetProductResult(PaginationResult<ProductDto> Products);

public class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.paginationRequest.pageIndex;
        var pageSize = query.paginationRequest.pageSize;
        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);

        var products = await dbContext.Products
                        .AsNoTracking()
                        .OrderBy(p => p.Name)
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<List<ProductDto>>();
        return new GetProductResult(new PaginationResult<ProductDto>(pageIndex, pageSize, totalCount, productDtos));
    }
}