namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery() : IQuery<GetProductResult>;

public record GetProductResult(IEnumerable<ProductDto> Products);

public class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
                        .AsNoTracking()
                        .OrderBy(p => p.Name)
                        .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<List<ProductDto>>();
        return new GetProductResult(productDtos);

    }

}
