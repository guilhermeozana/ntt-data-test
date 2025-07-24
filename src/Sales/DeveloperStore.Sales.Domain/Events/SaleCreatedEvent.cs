using DeveloperStore.Shared.SharedKernel.Domain.Events;

namespace DeveloperStore.Sales.Domain.Events;

public record SaleCreatedEvent(int SaleId, string SaleNumber, DateTime CreatedAt) : IDomainEvent
{
    public DateTime OccurredOn => CreatedAt;
}