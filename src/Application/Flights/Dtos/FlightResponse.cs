using Domain.Flights;

namespace Application.Flights.Dtos;

public record class FlightResponse(
	Guid Id,
	string Origin,
	string Destination,
	DateTime DepartureTime,
	DateTime ArrivalTime,
	string Airline,
	int SeatsAvailable,
	string Status)
{

	public static FlightResponse From(Flight flight)
	{
		return new FlightResponse(
			flight.Id,
			flight.Origin,
			flight.Destination,
			flight.DepartureTime,
			flight.ArrivalTime,
			flight.Airline,
			flight.SeatsAvailable,
			flight.Status.Name);
	}
}