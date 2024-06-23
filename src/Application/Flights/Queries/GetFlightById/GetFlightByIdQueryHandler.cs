using Application.Common.Interfaces;
using Application.Flights.Dtos;
using Domain.Common.Models;
using Domain.Flights;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Flights.Queries.GetFlightById;

public class GetFlightByIdQueryHandler(IAppDbContext _context) : IRequestHandler<GetFlightByIdQuery, Result<FlightResponse>>
{
	public async Task<Result<FlightResponse>> Handle(GetFlightByIdQuery request, CancellationToken cancellationToken)
	{
		var flight = await _context
			.Set<Flight>()
			.FirstOrDefaultAsync(flight => flight.Id == request.Id,
			cancellationToken);

		if (flight is null)
		{
			return Error.NotFound(description: "Flight not found.");
		}

		return FlightResponse.From(flight);
	}
}
