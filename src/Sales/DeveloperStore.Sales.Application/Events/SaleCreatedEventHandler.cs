using MediatR;
using DeveloperStore.Sales.Domain.Events;

namespace DeveloperStore.Sales.Application.Events;

public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale #{notification.SaleId} was created at {notification.OccurredOn:O}");
        return Task.CompletedTask;
    }
}