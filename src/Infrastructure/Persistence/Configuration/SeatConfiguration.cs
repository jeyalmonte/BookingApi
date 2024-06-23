using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
	public class SeatConfiguration : IEntityTypeConfiguration<Seat>
	{
		public void Configure(EntityTypeBuilder<Seat> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.SeatNumber).IsRequired();
			builder.Property(x => x.IsOccupied).IsRequired();
			builder.Property(x => x.FlightId).IsRequired();
		}
	}
}
