using Domain.Common;
using Domain.Flights;
using Domain.Reservations.Events;

namespace Domain.Reservations;

public sealed class Reservation : Entity<Guid>
{
	public string BookerName { get; private set; } = null!;
	public string BookerEmail { get; private set; } = null!;
	public Guid FlightId { get; set; }
	public Flight Flight { get; private set; } = null!;
	public DateTime BookingDate { get; private set; }
	public int SeatNumber { get; private set; }
	public ReservationStatus Status { get; private set; } = null!;

	public Reservation(
		Guid id,
		string passengerName,
		string passengerEmail,
		Guid flightId,
		DateTime bookingDate,
		int seatsNumber,
		ReservationStatus status) : base(id)
	{
		BookerName = passengerName;
		BookerEmail = passengerEmail;
		FlightId = flightId;
		BookingDate = bookingDate;
		SeatNumber = seatsNumber;
		Status = status;
	}

	public static Reservation Create(
		string passengerName,
		string passengerEmail,
		Guid flightId,
		int seatsNumber)
	{
		var reservation = new Reservation(
			Guid.NewGuid(),
			passengerName,
			passengerEmail,
			flightId,
			DateTime.UtcNow,
			seatsNumber,
			ReservationStatus.Booked);

		reservation.RaiseEvent(new ReservationCreatedDomainEvent(reservation.Id));

		return reservation;
	}

	public void CancelReservation()
	{
		Status = ReservationStatus.Cancelled;
		RaiseEvent(new ReservationCancelledDomainEvent(Id));
	}
	private Reservation() { }
}
