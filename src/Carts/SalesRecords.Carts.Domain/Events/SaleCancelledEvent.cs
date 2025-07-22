using SalesRecords.Shared.SharedKernel.Domain.Events;

namespace SalesRecords.Carts.Domain.Events;

public record SaleCancelledEvent(int CartId, DateTime CancelledAt) : IDomainEvent
{
    public DateTime OccurredOn => CancelledAt;
}