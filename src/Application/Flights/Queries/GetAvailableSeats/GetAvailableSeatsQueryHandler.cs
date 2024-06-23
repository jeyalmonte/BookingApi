using Application.Common.Interfaces;
using Domain.Common.Models;
using Domain.Flights;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Flights.Queries.GetAvailableSeats
{
	public class GetAvailableSeatsQueryHandler(IAppDbContext _context) : IRequestHandler<GetAvailableSeatsQuery, Result<int[]>>
	{
		public async Task<Result<int[]>> Handle(GetAvailableSeatsQuery request, CancellationToken cancellationToken)
		{
			var seats = await _context
				.Set<Seat>()
				.AsNoTracking()
				.Where(seat => seat.FlightId == request.FlightId && !seat.IsOccupied)
				.Select(seat => seat.SeatNumber)
				.ToArrayAsync(cancellationToken);

			if (seats.Length < 1)
			{
				return Error.Failure(description: "No available seats.");
			}

			return seats;
		}
	}
}
