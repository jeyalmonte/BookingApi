using Application.Flights.Queries.GetAvailableSeats;
using Application.Flights.Queries.GetFlightById;
using Application.Flights.Queries.GetFlights;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class FlightsController(ISender sender) : ApiController
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> Get([FromQuery] GetFlightsQuery query, CancellationToken cancellationToken)
		=> Ok(await sender.Send(query, cancellationToken));

	[HttpGet("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetFlightByIdQuery(id);
		var result = await sender.Send(query, cancellationToken);

		return result.Match(Ok, Problem);
	}

	[HttpGet("{id:guid}/seats")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetAvailableSeats(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetAvailableSeatsQuery(id);
		var result = await sender.Send(query, cancellationToken);
		return result.Match(Ok, Problem);
	}
}
