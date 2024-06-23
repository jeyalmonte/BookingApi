using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Context;

public class AppDbContextInitializer(AppDbContext _context, ILogger<AppDbContextInitializer> _logger)
{

	public async Task InitialiseAsync()
	{
		try
		{
			if (_context.Database.IsNpgsql())
			{
				await _context.Database.MigrateAsync();
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while initialising the database.");
		}
	}

	public async Task SeedAsync()
	{
		try
		{
			await TrySeedAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while seeding the database.");
		}
	}

	public async Task TrySeedAsync()
	{
		if (!_context.Flights.Any())
		{
			var flights = new List<Flight>
		{
			CreateFlight("SDQ", "JFK", "American Airlines", 14, 20, 15, 1, FligthStatus.Scheduled),
			CreateFlight("PUJ", "MIA", "Delta Airlines", 15, 2, 15, 5, FligthStatus.Scheduled),
			CreateFlight("STI", "ATL", "United Airlines", 15, 5, 15, 10, FligthStatus.Scheduled),
			CreateFlight("LAX", "SDQ", "Spirit Airlines", 15, 6, 15, 12, FligthStatus.Scheduled),
			CreateFlight("ORD", "PUJ", "JetBlue", 15, 10, 15, 18, FligthStatus.Scheduled)
		};

			_context.Flights.AddRange(flights);
			await _context.SaveChangesAsync();
		}
	}

	private static Flight CreateFlight(string origin, string destination, string airline, int departureDay, int departureHour, int arrivalDay, int arrivalHour, FligthStatus status)
	{
		int numberOfSeats = GetRandomNumbers();
		var seats = GetSeats(numberOfSeats);
		return new Flight(origin, destination, DateTime.UtcNow.AddDays(departureDay).AddHours(departureHour), DateTime.UtcNow.AddDays(arrivalDay).AddHours(arrivalHour), numberOfSeats, seats, airline, status);
	}

	private static int GetRandomNumbers()
	{
		var random = new Random();
		return random.Next(1, 25);
	}

	private static List<Seat> GetSeats(int seatsNumber)
	{
		var seats = new List<Seat>();
		for (int i = 1; i <= seatsNumber; i++)
		{
			seats.Add(new Seat(i));
		}
		return seats;
	}
}


