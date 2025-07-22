using MediatR;

namespace SalesRecords.Shared.SharedKernel.Domain.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}