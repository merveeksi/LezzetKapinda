using LezzetKapinda.DomainEvents;

namespace LezzetKapinda.Entities;

public abstract class EntityBase<TKey> : IEntity<TKey>
    where TKey : struct
{
    public virtual TKey Id { get; set; }
    public virtual string CreatedByUserId { get; set; } = string.Empty;
    public virtual DateTimeOffset CreatedOn { get; set; }
    public virtual string? LastModifiedByUserId { get; set; }
    public virtual DateTimeOffset? LastModifiedOn { get; set; }
    
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();
    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}