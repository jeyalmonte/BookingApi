using Domain.People;

namespace Application.People.Dtos;

public record PersonDto(
	Guid Id,
	string FirstName,
	string LastName,
	string Gender,
	string Email,
	string? PhoneNumber,
	string? Address)
{
	public static PersonDto FromPerson(Person person)
	{
		return new PersonDto(
			person.Id,
			person.FirstName,
			person.LastName,
			person.Gender.Name,
			person.Email,
			person.PhoneNumber,
			person.Address);
	}
}