using Domain.Reservations.Events;
using MassTransit;
using MediatR;

namespace Application.Reservations.Events;

public class ReservationCreatedDomainEventHandler(IBus bus)
	: INotificationHandler<ReservationCreatedDomainEvent>
{
	public async Task Handle(ReservationCreatedDomainEvent notification, CancellationToken cancellationToken)
	{
		await bus.Publish(new ReservationCreatedEvent(notification.ReservationId), cancellationToken);
	}

}

public record ReservationCreatedEvent(Guid ReservationId);