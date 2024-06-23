using Domain.Common.Models;
using MediatR;

namespace Application.Reservations.Commands.CancelReservation;

public record CancelReservationCommand(Guid Id) : IRequest<Result<Guid>>;
