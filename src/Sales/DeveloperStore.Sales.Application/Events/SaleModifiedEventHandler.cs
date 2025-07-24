using MediatR;
using DeveloperStore.Sales.Domain.Events;

namespace DeveloperStore.Sales.Application.Events;

public class SaleModifiedEventHandler : INotificationHandler<SaleModifiedEvent>
{
    public Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale #{notification.SaleId} was modified at {notification.OccurredOn:O}");
        return Task.CompletedTask;
    }
}