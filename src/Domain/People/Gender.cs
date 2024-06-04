using Ardalis.SmartEnum;

namespace Domain.People;

public class Gender(string name, int value)
	: SmartEnum<Gender>(name, value)
{
	public static readonly Gender NotSpecified = new(nameof(NotSpecified), 0);
	public static readonly Gender Male = new(nameof(Male), 1);
	public static readonly Gender Female = new(nameof(Female), 2);
}