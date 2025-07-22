using SalesRecords.Shared.SharedKernel.Domain.Events;

namespace SalesRecords.Carts.Domain.Events;

public record ItemCancelledEvent(int CartId, int ProductId, DateTime CancelledAt) : IDomainEvent
{
    public DateTime OccurredOn => CancelledAt;
}