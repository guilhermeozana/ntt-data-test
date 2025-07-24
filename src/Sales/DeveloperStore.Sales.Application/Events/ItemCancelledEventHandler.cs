using MediatR;
using DeveloperStore.Sales.Domain.Events;

namespace DeveloperStore.Sales.Application.Events;

public class ItemCancelledEventHandler : INotificationHandler<ItemCancelledEvent>
{
    public Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Item #{notification.ProductId} was cancelled in Cart #{notification.SaleId} at {notification.OccurredOn:O}");
        return Task.CompletedTask;
    }
}