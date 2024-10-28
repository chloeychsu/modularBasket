
using Catalog.Products.Dtos;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdReturn>;

public record GetProductByIdReturn(ProductDto Product);

public class GetProductByIdHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdReturn>
{
    public async Task<GetProductByIdReturn> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.AsNoTracking()
                        .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);
        if (product is null)
        {
            throw new Exception($"Product not found: {query.Id}");
        }
        var productDto = product.Adapt<ProductDto>();
        return new GetProductByIdReturn(productDto);
    }
}
