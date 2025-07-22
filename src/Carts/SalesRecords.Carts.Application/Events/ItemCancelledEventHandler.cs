using MediatR;
using SalesRecords.Carts.Domain.Events;

namespace SalesRecords.Carts.Application.Events;

public class ItemCancelledEventHandler : INotificationHandler<ItemCancelledEvent>
{
    public Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Item #{notification.ProductId} was cancelled in Cart #{notification.CartId} at {notification.OccurredOn:O}");
        return Task.CompletedTask;
    }
}