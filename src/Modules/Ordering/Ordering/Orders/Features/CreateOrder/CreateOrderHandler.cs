﻿namespace Ordering.Orders.Features.CreateOrder;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;
public record CreateOrderResult(Guid Id);
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("OrderName is required");
    }
}


public class CreateOrderHandler(OrderingDbContext context) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);

        context.Orders.Add(order);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id);
    }

    public Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(orderDto.Billingaddress.FirstName, orderDto.Billingaddress.LastName, orderDto.Billingaddress.EmailAddress, orderDto.Billingaddress.AddressLine, orderDto.Billingaddress.Country, orderDto.Billingaddress.State, orderDto.Billingaddress.ZipCode);

        var newOrder = Order.Create(
            id: Guid.NewGuid(),
            customerId: orderDto.CustomerId,
            orderName: $"{orderDto.OrderName}_{new Random().Next()}",
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod)
        );
        orderDto.Items.ForEach(item => newOrder.Add(item.ProductId, item.Quantity, item.Price));
        return newOrder;
    }
}