namespace WebportSystem.Common.Application.Abstractions;

public abstract class DomainEventDispatcher<TDomainEvent> : IDomainEventDispatcher<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public abstract Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);

    public Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken) =>
        Handle((TDomainEvent)domainEvent, cancellationToken);
}