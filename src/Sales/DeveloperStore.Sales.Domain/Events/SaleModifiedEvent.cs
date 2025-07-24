using DeveloperStore.Shared.SharedKernel.Domain.Events;

namespace DeveloperStore.Sales.Domain.Events;

public record SaleModifiedEvent(int SaleId, DateTime ModifiedAt) : IDomainEvent
{
    public DateTime OccurredOn => ModifiedAt;
}