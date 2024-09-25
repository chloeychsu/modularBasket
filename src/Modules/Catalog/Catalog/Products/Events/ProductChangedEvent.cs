namespace Catalog.Products.Events;

public record ProductChangedEvent(Product porduct):IDomainEvent;