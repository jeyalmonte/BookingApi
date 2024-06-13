namespace Domain.Flights.Extensions;

public static class SeatExtensions
{
	private static readonly Random _random = new();

	public static int ReserveRandomSeat(this List<Seat> seats)
	{
		var availableSeats = seats.Where(s => !s.IsOccupied).ToList();

		if (availableSeats.Count == 0)
		{
			return 0;
		}

		var randomIndex = _random.Next(availableSeats.Count);
		var selectedSeat = availableSeats[randomIndex];

		selectedSeat.Reserve();

		return selectedSeat.SeatNumber;
	}
}
