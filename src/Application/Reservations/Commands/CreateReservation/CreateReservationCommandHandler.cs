using Application.Common.Interfaces;
using Domain.Common.Models;
using Domain.Flights;
using Domain.Reservations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler(IAppDbContext _context)
	: IRequestHandler<CreateReservationCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
	{
		var flight = await _context
			.Set<Flight>()
			.Include(flight => flight.Seats)
			.SingleOrDefaultAsync(x => x.Id == request.FlightId,
			 cancellationToken: cancellationToken);

		if (flight is null)
		{
			return Error.Failure(description: "Flight not found.");
		}

		if (flight.SeatsAvailable <= 0)
		{
			return Error.Failure(description: "Not enough available seats.");
		}

		int seat = request.SeatNumber;

		if (!IsSeatAvailable(flight, ref seat))
		{
			return Error.Failure(description: "Seat not available.");
		}

		var reservation = Reservation.Create(
			request.PassengerName,
			request.PassengerEmail,
			request.FlightId,
			seat);

		_context.Set<Reservation>().Add(reservation);

		await _context.SaveChangesAsync(cancellationToken);

		return reservation.Id;
	}

	private static bool IsSeatAvailable(Flight flight, ref int seatNumber)
	{
		if (seatNumber > 0)
		{
			return flight.ReserveSeat(seatNumber);
		}

		seatNumber = flight.ReserveAnySeat();
		return seatNumber > 0;
	}
}
