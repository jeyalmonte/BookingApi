using Application.Common.Interfaces;
using Application.Flights.Dtos;
using Domain.Flights;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Flights.Queries.GetFlights;

public class GetFlightsQueryHandler(IAppDbContext _dbContext)
	: IRequestHandler<GetFlightsQuery, IReadOnlyList<FlightResponse>>
{
	public async Task<IReadOnlyList<FlightResponse>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
	{
		var status = FligthStatus.FromName(
			name: request.Status ?? FligthStatus.Scheduled.Name,
			ignoreCase: true);

		var flights = await _dbContext
			.Set<Flight>()
			.AsNoTracking()
			.Where(flight => flight.Status == status)
			.Select(flight => FlightResponse.From(flight))
			.ToListAsync(cancellationToken);

		return flights;
	}
}
