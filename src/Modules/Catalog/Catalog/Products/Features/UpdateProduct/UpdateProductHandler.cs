﻿namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product):ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greter then 0");
    }
}
public class UpdateProductHandler(CatalogDbContext dbContext) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        // 1. check 
        var product = await dbContext.Products.FindAsync([command.Product.Id],cancellationToken);

        if(product is null){
            throw new ProductNotFoundException(command.Product.Id);
        }
        // 2.update
        var productDto = command.Product;
        product.Update(productDto.Name,productDto.Category,productDto.Description,productDto.ImageFile,productDto.Price);

        // 3. return
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
