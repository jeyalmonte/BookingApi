using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Reservations.Events;
using Domain.Reservations;
using Infrastructure.Persistence.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Consumers;

public class ReservationCreatedConsumer(
	AppDbContext _context,
	IEmailService _emailService,
	ILogger<ReservationCreatedEvent> _logger)
	: IConsumer<ReservationCreatedEvent>
{
	public async Task Consume(ConsumeContext<ReservationCreatedEvent> context)
	{
		var reservation = await _context
			.Set<Reservation>()
			.AsNoTracking()
			.Include(Reservation => Reservation.Flight)
			.SingleOrDefaultAsync(x => x.Id == context.Message.ReservationId,
			context.CancellationToken);

		if (reservation is null)
		{
			_logger.LogError("Reservation with {ReservationId} not found", context.Message.ReservationId);
			return;
		}

		var properties = new Dictionary<string, string>
		{
			{ "RESERVATION_ID", reservation.Id.ToString() },
			{ "PASSENGER_NAME", reservation.BookerName },
			{ "DEPARTURE_TIME", reservation.Flight.DepartureTime.ToString() },
			{ "ORIGIN", reservation.Flight.Origin },
			{ "DESTINATION", reservation.Flight.Destination },
			{ "SEAT_NUMBER", reservation.SeatNumber.ToString() }
		};

		var htmlContent = EmailHelper.GetTemplate("reservation-confirmation", properties);

		var IsEmailSent = await _emailService.SendAsync(
			to: reservation.BookerEmail,
			subject: "Reservation Confirmation",
			content: htmlContent,
			cancellation: context.CancellationToken);

		if (!IsEmailSent)
		{
			_logger.LogError("Failed to send reservation confirmation email: {ReservationId}", reservation.Id);
		}

		_logger.LogInformation("Reservation confirmation email sent successfully.");
	}
}
