using MediatR;
using SalesRecords.Shared.SharedKernel.Domain.Events;

namespace SalesRecords.Carts.Application.Common.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchEventsAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var domainEvent in events)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}