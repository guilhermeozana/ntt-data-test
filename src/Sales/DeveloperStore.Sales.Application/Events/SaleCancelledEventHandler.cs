using MediatR;
using DeveloperStore.Sales.Domain.Events;

namespace DeveloperStore.Sales.Application.Events;

public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledEvent>
{
    public Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Sale #{notification.SaleId} was cancelled at {notification.OccurredOn:O}");
        return Task.CompletedTask;
    }
}