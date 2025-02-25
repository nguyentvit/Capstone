using MediatR;

namespace BuildingBlocks.Abstractions;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IDomainEvent
{
}