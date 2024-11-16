using MassTransit;
using Shared.Messaging.Events;

namespace Catalog;

public class ProductPriceChangedEventHandler(IBus bus,ILogger<ProductPriceChangedEventHandler> logger) : INotificationHandler<ProductChangedEvent>
{
     public async Task Handle(ProductChangedEvent notification, CancellationToken cancellationToken)
     {
          logger.LogInformation("Domain Event handler: {DomainEvent}", notification.GetType().Name);
          // Integration Events
          var integrationEvent = new ProductPriceChangedIntegrationEvent
          {
               ProductId = notification.product.Id,
               Name = notification.product.Name,
               Category = notification.product.Category,
               Description = notification.product.Description,
               ImageFile = notification.product.ImageFile,
               Price = notification.product.Price
          };
          await bus.Publish(integrationEvent,cancellationToken);
     }
}
