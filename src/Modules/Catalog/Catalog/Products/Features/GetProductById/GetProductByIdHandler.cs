namespace Catalog.Products.Features.GetProductById;

public class GetProductByIdHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdReturn>
{
    public async Task<GetProductByIdReturn> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.AsNoTracking()
                        .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(query.Id);

        var productDto = product.Adapt<ProductDto>();
        return new GetProductByIdReturn(productDto);
    }
}