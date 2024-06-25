using Application.Common.Models;
using Application.Reservations.Dtos;
using MediatR;

namespace Application.Reservations.Queries.GetReservationList;

public record GetReservationListQuery : PaginateQuery, IRequest<PaginatedResult<ReservationResponse>>;
