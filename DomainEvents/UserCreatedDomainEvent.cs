namespace LezzetKapinda.DomainEvents;

public sealed record UserCreatedDomainEvent(long UserId) : IDomainEvent;