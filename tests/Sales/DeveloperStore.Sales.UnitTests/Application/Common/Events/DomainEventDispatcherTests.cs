using FluentAssertions;
using MediatR;
using NSubstitute;
using DeveloperStore.Sales.Application.Common.Events;
using DeveloperStore.Shared.SharedKernel.Domain.Events;

namespace DeveloperStore.Sales.UnitTests.Application.Common.Events;

public class DomainEventDispatcherTests
{
    [Fact]
    public async Task DispatchEventsAsync_ShouldPublishAllEvents()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var dispatcher = new DomainEventDispatcher(mediator);

        var events = new List<IDomainEvent>
        {
            new SaleTestEvent(),
            new ItemTestEvent()
        };

        // Act
        await dispatcher.DispatchEventsAsync(events);

        // Assert
        foreach (var domainEvent in events) await mediator.Received(1).Publish(domainEvent);
    }

    [Fact]
    public async Task DispatchEventsAsync_ShouldNotThrow_WhenListIsEmpty()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var dispatcher = new DomainEventDispatcher(mediator);
        var emptyEvents = new List<IDomainEvent>();

        // Act
        var act = async () => await dispatcher.DispatchEventsAsync(emptyEvents);

        // Assert
        await act.Should().NotThrowAsync();
        await mediator.DidNotReceive().Publish(Arg.Any<IDomainEvent>());
    }

    private class SaleTestEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    private class ItemTestEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}