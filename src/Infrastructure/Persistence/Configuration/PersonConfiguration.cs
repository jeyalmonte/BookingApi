using Domain.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
	public void Configure(EntityTypeBuilder<Person> builder)
	{
		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id)
			.ValueGeneratedNever();

		builder.Property(p => p.Gender)
			.HasConversion(
				v => v.Name,
				v => Gender.FromName(v, false));

		builder.Property(p => p.FirstName);
		builder.Property(p => p.LastName);
		builder.Property(p => p.Email);
		builder.Property(p => p.PhoneNumber);
		builder.Property(p => p.Address);
	}
}
