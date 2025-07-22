using MediatR;
using SalesRecords.Carts.Domain.Events;

namespace SalesRecords.Carts.Application.Events;

public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale #{notification.CartId} was created at {notification.OccurredOn:O}");
        return Task.CompletedTask;
    }
}