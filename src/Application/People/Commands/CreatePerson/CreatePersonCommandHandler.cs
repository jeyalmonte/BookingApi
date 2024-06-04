using Application.Common.Interfaces;
using Domain.Common.Models;
using Domain.People;
using MediatR;

namespace Application.People.Commands.CreatePerson
{
	public class CreatePersonCommandHandler(
		IAppDbContext _context) : IRequestHandler<CreatePersonCommand, Result<Guid>>
	{
		public async Task<Result<Guid>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
		{
			if (!Gender.TryFromName(request.Gender.ToString(), out var gender))
			{
				return Error.Failure(description: "Gender is not valid.");
			}

			var user = Person.Create(
				Guid.NewGuid(),
				request.FirstName,
				request.LastName,
				gender,
				request.Email,
				request.PhoneNumber,
				request.Address);

			_context.People.Add(user);

			await _context.SaveChangesAsync(cancellationToken);

			return user.Id;
		}
	}
}
