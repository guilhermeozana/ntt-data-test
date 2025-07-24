using DeveloperStore.Shared.SharedKernel.Domain.Events;

namespace DeveloperStore.Sales.Domain.Events;

public record ItemCancelledEvent(int SaleId, int ProductId, DateTime CancelledAt) : IDomainEvent
{
    public DateTime OccurredOn => CancelledAt;
}