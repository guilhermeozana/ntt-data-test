using SalesRecords.Shared.SharedKernel.Domain.Events;

namespace SalesRecords.Carts.Application.Common.Events;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(IEnumerable<IDomainEvent> events);
}