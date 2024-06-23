using Application.Reservations.Dtos;
using Domain.Common.Models;
using MediatR;

namespace Application.Reservations.Queries.GetReservationById;

public record GetReservationByIdQuery(Guid Id) : IRequest<Result<ReservationResponse>>;
