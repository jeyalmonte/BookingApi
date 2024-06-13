using Domain.Common;

namespace Domain.Flights;

public sealed class Seat : Entity<Guid>
{
	public int SeatNumber { get; private set; }
	public bool IsOccupied { get; private set; }
	public Guid FlightId { get; private set; }
	public Flight Flight { get; private set; } = null!;

	public Seat(int seatNumber) : base(Guid.NewGuid())
	{
		SeatNumber = seatNumber;
		IsOccupied = false;
	}

	public void Reserve()
	{
		IsOccupied = true;
	}

	public void Release()
	{
		IsOccupied = false;
	}

	private Seat()
	{
	}
}
