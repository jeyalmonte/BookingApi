using FluentValidation;

namespace Application.People.Commands.CreatePerson
{
	public class CreatePersonValidator : AbstractValidator<CreatePersonCommand>
	{
		public CreatePersonValidator()
		{
			RuleFor(x => x.FirstName)
				//.WithName("First Name")
				.NotEmpty().WithMessage("First Name is required.")
				.MaximumLength(50).WithMessage("First Name must not exceed 50 characters.");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("Last Name is required.")
				.MaximumLength(50).WithMessage("Last Name must not exceed 50 characters.");
		}
	}
}
