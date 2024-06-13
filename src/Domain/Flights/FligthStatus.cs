using Ardalis.SmartEnum;

namespace Domain.Flights;

public class FligthStatus(string Name, int Value)
	: SmartEnum<FligthStatus>(Name, Value)
{
	public static readonly FligthStatus Scheduled = new(nameof(Scheduled), 0);
	public static readonly FligthStatus Full = new(nameof(Full), 1);
	public static readonly FligthStatus InFlight = new(nameof(InFlight), 2);
	public static readonly FligthStatus Delayed = new(nameof(Delayed), 3);
	public static readonly FligthStatus Cancelled = new(nameof(Cancelled), 4);
	public static readonly FligthStatus Completed = new(nameof(Completed), 5);
}
