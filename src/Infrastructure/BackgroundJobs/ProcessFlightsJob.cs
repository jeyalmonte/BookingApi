using Application.Common.Interfaces;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Infrastructure.BackgroundJobs;

public class ProcessFlightsJob(
	IAppDbContext _context,
	IDateTimeProvider _dateTime) : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		var flights = await _context
			.Set<Flight>()
			.Where(flight =>
				   flight.Status != FligthStatus.Cancelled
				&& flight.Status != FligthStatus.Completed
				&& flight.Status != FligthStatus.Delayed)
			.Take(20)
			.ToListAsync(context.CancellationToken);

		foreach (var flight in flights)
		{
			ProcessFlight(flight);
		}

		await _context.SaveChangesAsync(context.CancellationToken);
	}

	private void ProcessFlight(Flight flight)
	{
		if (flight.SeatsAvailable <= 0 && flight.Status == FligthStatus.Scheduled)
		{
			flight.UpdateFlightStatus(FligthStatus.Full);
		}
		else if (flight.SeatsAvailable > 0 && flight.Status == FligthStatus.Full)
		{
			flight.UpdateFlightStatus(FligthStatus.InFlight);

		}
		else if (_dateTime.UtcNow > flight.DepartureTime
			&& (flight.Status == FligthStatus.Full || flight.Status == FligthStatus.Scheduled))
		{
			flight.UpdateFlightStatus(FligthStatus.InFlight);
		}
	}
}
