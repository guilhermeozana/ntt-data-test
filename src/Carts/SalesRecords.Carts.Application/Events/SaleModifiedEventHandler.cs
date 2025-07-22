using MediatR;
using SalesRecords.Carts.Domain.Events;

namespace SalesRecords.Carts.Application.Events;

public class SaleModifiedEventHandler : INotificationHandler<SaleModifiedEvent>
{
    public Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale #{notification.CartId} was modified at {notification.OccurredOn:O}");
        return Task.CompletedTask;
    }
}