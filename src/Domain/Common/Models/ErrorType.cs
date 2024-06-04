namespace Domain.Common.Models;

public enum ErrorType
{
	None,
	Failure,
	Validation,
	Unexpected,
	Conflict,
	NotFound,
	Unauthorized,
	Forbidden,
}
