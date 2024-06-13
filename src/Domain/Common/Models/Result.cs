namespace Domain.Common.Models;

public readonly record struct Result<TValue> : IResult<TValue>
{
	private Result(TValue? value = default, List<Error>? errors = default, bool isError = true)
	{
		_errors = errors;
		_value = value;
		IsError = isError;
	}


	public static implicit operator Result<TValue>(TValue value)
	{
		return new Result<TValue>(value: value, isError: false);
	}

	public static implicit operator Result<TValue>(Error error)
	{
		return new Result<TValue>(errors: [error]);
	}

	public static implicit operator Result<TValue>(List<Error> errors)
	{
		return new Result<TValue>(errors: errors);
	}

	public static implicit operator Result<TValue>(Error[] errors)
	{
		return new Result<TValue>(errors: [.. errors]);
	}

	public TValue Value => _value!;
	public bool IsError { get; }
	public List<Error> Errors
	{
		get
		{
			if (!IsError)
			{
				return [Error.None];
			}

			return _errors!;
		}
	}

	private readonly List<Error>? _errors;
	private readonly TValue? _value;

	public TResult Match<TResult>(Func<TValue, TResult> onValue, Func<List<Error>, TResult> onError)
	{
		if (IsError)
		{
			return onError(Errors);
		}

		return onValue(Value);
	}
}