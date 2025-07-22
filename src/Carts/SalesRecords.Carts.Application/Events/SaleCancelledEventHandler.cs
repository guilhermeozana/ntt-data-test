using MediatR;
using SalesRecords.Carts.Domain.Events;

namespace SalesRecords.Carts.Application.Events;

public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledEvent>
{
    public Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale #{notification.CartId} was cancelled at {notification.OccurredOn:O}");
        return Task.CompletedTask;
    }
}