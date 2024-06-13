using Domain.Common;
using Domain.Flights.Extensions;
using Domain.Reservations;

namespace Domain.Flights;

public sealed class Flight : Entity<Guid>
{
	public string Origin { get; private set; } = null!;
	public string Destination { get; private set; } = null!;
	public DateTime DepartureTime { get; private set; }
	public DateTime ArrivalTime { get; private set; }
	public string Airline { get; private set; } = null!;
	public int SeatsAvailable { get; private set; }
	public FligthStatus Status { get; private set; } = null!;

	private readonly List<Reservation> _reservations = [];
	private readonly List<Seat> _seats = [];
	public IReadOnlyCollection<Reservation> Reservations => _reservations;
	public IReadOnlyCollection<Seat> Seats => _seats;
	public Flight(
		string origin,
		string destination,
		DateTime departureTime,
		DateTime arrivalTime,
		int seatsAvailable,
		List<Seat> seats,
		string airline,
		FligthStatus status) : base(Guid.NewGuid())
	{
		Origin = origin;
		Destination = destination;
		DepartureTime = departureTime;
		ArrivalTime = arrivalTime;
		SeatsAvailable = seatsAvailable;
		Airline = airline;
		Status = status;
		_seats = seats;
	}

	public bool IsFull => SeatsAvailable == 0;

	public void UpdateFlightStatus(FligthStatus status)
	{
		Status = status;
	}


	public bool ReserveSeat(int seatNumber)
	{
		var seat = _seats.Find(s => s.SeatNumber == seatNumber);
		if (seat is null || seat.IsOccupied)
		{
			return false;
		}

		seat.Reserve();
		SeatsAvailable--;
		return true;
	}

	public bool ReleaseSeat(int seatNumber)
	{
		var seat = _seats.Find(s => s.SeatNumber == seatNumber);
		if (seat == null || !seat.IsOccupied)
		{
			return false;
		}

		seat.Release();
		SeatsAvailable++;

		return true;
	}

	public int ReserveAnySeat()
	{
		var seat = _seats.ReserveRandomSeat();
		if (seat > 0)
		{
			SeatsAvailable--;
			return seat;
		}
		return 0;
	}

	private Flight()
	{
	}
}
