using Application.Common.Interfaces;
using Application.People.Dtos;
using Domain.Common.Models;
using MediatR;

namespace Application.People.Queries.GetPerson
{
	public class GetPersonByIdQueryHandler(IAppDbContext _context) : IRequestHandler<GetPersonByIdQuery, Result<PersonDto>>
	{
		public async Task<Result<PersonDto>> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
		{
			var person = await _context.People
				.FindAsync([request.Id], cancellationToken: cancellationToken);

			if (person is null)
			{
				return Error.NotFound(description: $"Person not found with Id: {request.Id}");
			}

			return PersonDto.FromPerson(person);
		}
	}
}
