namespace Catalog.Products.Events;

public record ProductChangedEvent(Product product):IDomainEvent;