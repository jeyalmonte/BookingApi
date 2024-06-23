using Domain.Reservations;

namespace Application.Reservations.Dtos;

public record ReservationResponse(
	Guid Id,
	string BookerName,
	string BookerEmail,
	string Origin,
	string Destination,
	DateTime DepartureTime,
	DateTime ArrivalTime,
	string Airline,
	string Status,
	int SeatsNumber,
	DateTime BookingDate)
{

	public static ReservationResponse From(Reservation reservation)
	{
		return new ReservationResponse(
			reservation.Id,
			reservation.BookerName,
			reservation.BookerEmail,
			reservation.Flight.Origin,
			reservation.Flight.Destination,
			reservation.Flight.DepartureTime,
			reservation.Flight.ArrivalTime,
			reservation.Flight.Airline,
			reservation.Status.Name,
			reservation.SeatNumber,
			reservation.BookingDate);
	}
}

