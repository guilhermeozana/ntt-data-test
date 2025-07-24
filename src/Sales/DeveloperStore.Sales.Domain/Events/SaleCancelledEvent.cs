using DeveloperStore.Shared.SharedKernel.Domain.Events;

namespace DeveloperStore.Sales.Domain.Events;

public record SaleCancelledEvent(int SaleId, DateTime CancelledAt) : IDomainEvent
{
    public DateTime OccurredOn => CancelledAt;
}