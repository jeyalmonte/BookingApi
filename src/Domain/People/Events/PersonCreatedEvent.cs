using Domain.Common;

namespace Domain.People.Events;


public record PersonCreatedEvent(Person Person) : IDomainEvent;