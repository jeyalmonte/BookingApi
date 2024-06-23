using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
	public void Configure(EntityTypeBuilder<Flight> builder)
	{
		builder.HasKey(f => f.Id);

		builder.Property(f => f.Id)
			.ValueGeneratedNever();

		builder.Property(f => f.Origin)
			.HasMaxLength(3);

		builder.Property(f => f.Destination)
			.HasMaxLength(3);

		builder.Property(f => f.DepartureTime);

		builder.Property(f => f.ArrivalTime);

		builder.Property(f => f.Airline)
			.HasMaxLength(100);

		builder.Property(f => f.Status)
		.HasConversion(
			v => v.Name,
			v => FligthStatus.FromName(v, false));

		builder.Property(f => f.SeatsAvailable);

		builder.HasMany(f => f.Reservations)
			.WithOne(x => x.Flight)
			.HasForeignKey(r => r.FlightId)
			.OnDelete(DeleteBehavior.Cascade);
	}

}
