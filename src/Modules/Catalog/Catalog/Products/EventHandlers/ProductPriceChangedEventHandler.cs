namespace Catalog;

public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger) : INotificationHandler<ProductChangedEvent>
{
     public Task Handle(ProductChangedEvent notification, CancellationToken cancellationToken)
     {
          logger.LogInformation("Domain Event handler: {DomainEvent}", notification.GetType().Name);
          return Task.CompletedTask;
     }
}
