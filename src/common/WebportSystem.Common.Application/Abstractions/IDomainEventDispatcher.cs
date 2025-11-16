namespace WebportSystem.Common.Application.Abstractions;

public interface IDomainEventDispatcher<in TDomainEvent> : IDomainEventDispatcher
    where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
}

public interface IDomainEventDispatcher
{
    Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken);
}