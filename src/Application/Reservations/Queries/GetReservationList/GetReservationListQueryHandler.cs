using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Reservations.Dtos;
using Domain.Reservations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reservations.Queries.GetReservationList;

public class GetReservationListQueryHandler(IAppDbContext _appContext)
	: IRequestHandler<GetReservationListQuery, PaginatedResult<ReservationResponse>>
{
	public async Task<PaginatedResult<ReservationResponse>> Handle(GetReservationListQuery request, CancellationToken cancellationToken)
	{
		var reservations = await _appContext
				.Set<Reservation>()
				.Include(reservation => reservation.Flight)
				.Select(reservation => ReservationResponse.From(reservation))
				.PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

		return reservations;

	}
}
