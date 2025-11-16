namespace WebportSystem.Common.Domain.Abstractions;

public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected BaseEntity()
    {
    }

    public bool IsActive { get; private set; } = true;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents];

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}