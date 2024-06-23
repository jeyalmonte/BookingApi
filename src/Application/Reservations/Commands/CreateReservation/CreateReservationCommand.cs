using Domain.Common.Models;
using MediatR;

namespace Application.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(
	string PassengerName,
	string PassengerEmail,
	Guid FlightId,
	int SeatNumber) : IRequest<Result<Guid>>;
