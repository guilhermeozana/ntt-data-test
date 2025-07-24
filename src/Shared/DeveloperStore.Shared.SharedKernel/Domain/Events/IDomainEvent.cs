using MediatR;

namespace DeveloperStore.Shared.SharedKernel.Domain.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}