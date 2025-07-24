using DeveloperStore.Shared.SharedKernel.Domain.Events;

namespace DeveloperStore.Sales.Application.Common.Events;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(IEnumerable<IDomainEvent> events);
}