using Domain.Common;

namespace Domain.Reservations.Events;

public record ReservationCancelledDomainEvent(Guid ReservationId) : IDomainEvent;