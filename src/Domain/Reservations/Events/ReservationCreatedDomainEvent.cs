using Domain.Common;

namespace Domain.Reservations.Events;

public record ReservationCreatedDomainEvent(Guid ReservationId) : IDomainEvent;

