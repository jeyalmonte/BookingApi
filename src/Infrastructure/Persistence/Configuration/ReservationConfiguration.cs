using Domain.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
	public void Configure(EntityTypeBuilder<Reservation> builder)
	{
		builder.HasKey(r => r.Id);

		builder.Property(r => r.Id)
			.ValueGeneratedNever();

		builder.Property(r => r.BookerName);

		builder.Property(r => r.BookerEmail);

		builder.Property(r => r.Status)
			.HasConversion(
				v => v.Name,
				v => ReservationStatus.FromName(v, false));

		builder.Property(r => r.BookingDate);

		builder.Property(r => r.SeatNumber);

		builder.HasOne(r => r.Flight)
			.WithMany(f => f.Reservations)
			.HasForeignKey(r => r.FlightId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
