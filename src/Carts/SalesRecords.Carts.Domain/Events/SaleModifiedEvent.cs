using SalesRecords.Shared.SharedKernel.Domain.Events;

namespace SalesRecords.Carts.Domain.Events;

public record SaleModifiedEvent(int CartId, DateTime ModifiedAt) : IDomainEvent
{
    public DateTime OccurredOn => ModifiedAt;
}