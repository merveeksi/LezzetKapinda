namespace LezzetKapinda.DomainEvents;

public record AddressCreatedDomainEvent(long AddressId) : IDomainEvent;