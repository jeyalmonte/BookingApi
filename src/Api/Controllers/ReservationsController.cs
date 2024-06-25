using Application.Reservations.Commands.CancelReservation;
using Application.Reservations.Commands.CreateReservation;
using Application.Reservations.Queries.GetReservationById;
using Application.Reservations.Queries.GetReservationList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class ReservationsController(ISender _sender) : ApiController
{

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([FromBody] CreateReservationCommand command)
	{
		var result = await _sender.Send(command);

		return result.Match(
			id => CreatedAtAction(nameof(GetById), new { id }, default),
			Problem);
	}
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> GetList([FromQuery] GetReservationListQuery query, CancellationToken cancellationToken)
	{
		var result = await _sender.Send(query, cancellationToken);
		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var result = await _sender.Send(
			new GetReservationByIdQuery(id),
			cancellationToken);

		return result.Match(Ok, Problem);
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> CancelReservation(Guid id, CancellationToken cancellationToken)
	{
		var result = await _sender.Send(
			new CancelReservationCommand(id),
			cancellationToken);

		return result.Match(
			_ => NoContent(),
			Problem);
	}

}
