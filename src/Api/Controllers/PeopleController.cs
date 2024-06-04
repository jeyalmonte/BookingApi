using Application.People.Commands.CreatePerson;
using Application.People.Dtos;
using Application.People.Queries.GetPerson;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
	[Route("api/people")]
	public class PeopleController(ISender _mediator) : ApiController
	{
		[HttpPost]
		[SwaggerOperation("Create a new person.")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreatePerson([FromBody] CreatePersonCommand command)
		{
			var result = await _mediator.Send(command);

			return result.Match(
				personId => CreatedAtAction(nameof(GetPerson), new { personId }, null),
				Problem);
		}

		[HttpGet("{personId:guid}")]
		[SwaggerOperation("Get person by id.")]
		[ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetPerson(Guid personId)
		{
			var result = await _mediator.Send(
				new GetPersonByIdQuery(personId));

			return result.Match(Ok, Problem);
		}

	}
}
