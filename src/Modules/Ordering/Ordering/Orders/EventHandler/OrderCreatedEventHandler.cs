
namespace Ordering.Orders.EventHancler;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Domain Event handled:{notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
