using Ardalis.SmartEnum;

namespace Domain.Reservations;

public class ReservationStatus(string name, int value)
	: SmartEnum<ReservationStatus>(name, value)
{
	public static readonly ReservationStatus Booked = new(nameof(Booked), 0);
	public static readonly ReservationStatus Completed = new(nameof(Completed), 1);
	public static readonly ReservationStatus Cancelled = new(nameof(Cancelled), 2);
}

