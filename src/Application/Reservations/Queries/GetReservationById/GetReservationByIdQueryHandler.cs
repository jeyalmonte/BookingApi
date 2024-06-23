using Application.Common.Interfaces;
using Application.Reservations.Dtos;
using Domain.Common.Models;
using Domain.Reservations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reservations.Queries.GetReservationById
{
	public class GetReservationByIdQueryHandler(IAppDbContext _context)
		: IRequestHandler<GetReservationByIdQuery, Result<ReservationResponse>>
	{
		public async Task<Result<ReservationResponse>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
		{
			var reservation = await _context
				.Set<Reservation>()
				.AsNoTracking()
				.Include(reservation => reservation.Flight)
				.SingleOrDefaultAsync(reservation => reservation.Id == request.Id,
				cancellationToken);

			if (reservation is null)
			{
				return Error.NotFound(description: "Reservation not found.");
			}

			return ReservationResponse.From(reservation);
		}
	}

}
