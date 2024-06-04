namespace Domain.Common.Models;

public interface IResult
{
	List<Error>? Errors { get; }
	bool IsError { get; }
}
public interface IResult<out TValue> : IResult
{
	TValue Value { get; }
}
