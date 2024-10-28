namespace Catalog.Products.Features.GetProductByCategory;

public record GetProductByCatagoryQuery(string Category) : IQuery<GetProductByCatagoryResult>;

public record GetProductByCatagoryResult(IEnumerable<ProductDto> Products);

public class GetProductByCategoryHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByCatagoryQuery, GetProductByCatagoryResult>
{
    public async Task<GetProductByCatagoryResult> Handle(GetProductByCatagoryQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products.AsNoTracking()
        .Where(p => p.Category.Contains(query.Category))
        .OrderBy(p => p.Name)
        .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<List<ProductDto>>();

        return new GetProductByCatagoryResult(productDtos);
    }
}
