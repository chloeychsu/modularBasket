namespace Ordering.Orders.Modules;

public class OrderItem:Entity<Guid>
{
    public Guid OrderId { get; private set; } = default!;
    public Guid ProductId { get; private set; } = default!;
    public int Quantity { get; internal set; } = default!;
    public decimal Price { get; private set; } = default!;
    public OrderItem(Guid orderId,Guid productId,int quantity,decimal price)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
}
