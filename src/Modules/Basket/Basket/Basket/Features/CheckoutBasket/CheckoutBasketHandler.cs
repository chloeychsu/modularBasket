using System.Text.Json;
using Shared.Messaging.Events;

namespace Basket.Basket.Features.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);
public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketHandler(BasketDbContext dbContext) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        ////// Checkout Basket Without Outbox ---- START
        // var basket = await repository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken);
        // var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
        // eventMessage.TotalPrice = basket.TotalPrice;
        // await bus.Publish(eventMessage,cancellationToken);
        // await repository.DeleteBasket(command.BasketCheckout.UserName,cancellationToken);
        // return new CheckoutBasketResult(true);
        ////// Checkout Basket Without Outbox ---- END
        ///

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // 取得當前在購物車的物品和總金額
            var basket = await dbContext.ShoppingCarts.Include(x => x.Items).SingleOrDefaultAsync(x => x.UserName == command.BasketCheckout.UserName, cancellationToken);
            if (basket == null)
            {
                throw new BasketNotFoundException(command.BasketCheckout.UserName);
            }
            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;
            // 寫入 Outbox Table
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage),
                OccuredOn = DateTime.UtcNow
            };
            dbContext.OutboxMessages.Add(outboxMessage);
            // Delete the basket
            dbContext.ShoppingCarts.Remove(basket);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new CheckoutBasketResult(true);
        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CheckoutBasketResult(false);
        }
    }
}
