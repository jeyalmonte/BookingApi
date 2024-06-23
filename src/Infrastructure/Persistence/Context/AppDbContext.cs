using Application.Common.Interfaces;
using Domain.Flights;
using Domain.Reservations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public class AppDbContext(DbContextOptions options) : DbContext(options), IAppDbContext
{
	public DbSet<Flight> Flights => Set<Flight>();
	public DbSet<Reservation> Reservations => Set<Reservation>();
	public DbSet<Seat> Seats => Set<Seat>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}
}
