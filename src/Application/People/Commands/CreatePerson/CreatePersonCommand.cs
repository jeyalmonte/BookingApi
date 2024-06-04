using Domain.Common.Models;
using Domain.People.Enums;
using MediatR;

namespace Application.People.Commands.CreatePerson;

public record CreatePersonCommand(
	string FirstName,
	string LastName,
	GenderType Gender,
	string Email,
	string? PhoneNumber,
	string? Address) : IRequest<Result<Guid>>;
