using SalesRecords.Shared.SharedKernel.Domain.Events;

namespace SalesRecords.Carts.Domain.Events;

public record SaleCreatedEvent(int CartId, DateTime CreatedAt) : IDomainEvent
{
    public DateTime OccurredOn => CreatedAt;
}