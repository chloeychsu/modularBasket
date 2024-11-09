﻿namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;

public record CreateBasketResult(Guid Id);

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CreateBasketHandler(BasketDbContext dbContext) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var newBasket = ShoppingCart.Create(Guid.NewGuid(), command.ShoppingCart.UserName);
        command.ShoppingCart.Items.ForEach(x => newBasket.AddItem(x.ProductId, x.Quantity, x.Color, x.Price, x.ProductName));

        dbContext.ShoppingCarts.Add(newBasket);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateBasketResult(newBasket.Id);
    }
}