using Domain.Common;
using Domain.People.Events;

namespace Domain.People;

public class Person : BaseEntity
{
	public string FirstName { get; } = null!;
	public string LastName { get; } = null!;
	public Gender Gender { get; private set; } = null!;
	public string Email { get; } = null!;
	public string? PhoneNumber { get; }
	public string? Address { get; }

	public Person(
		Guid id,
		string firstName,
		string lastName,
		Gender gender,
		string email,
		string? phoneNumber,
		string? address) : base(id)
	{
		FirstName = firstName;
		LastName = lastName;
		Gender = gender;
		Email = email;
		PhoneNumber = phoneNumber;
		Address = address;
	}

	public static Person Create(
		Guid id,
		string firstName,
		string lastName,
		Gender gender,
		string email,
		string? phoneNumber,
		string? address)
	{
		var person = new Person(
			id,
			firstName,
			lastName,
			gender,
			email,
			phoneNumber,
			address);

		person._domainEvents.Add(new PersonCreatedEvent(person));
		return person;
	}
	private Person() { }
}

