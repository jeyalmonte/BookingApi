using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services;

public class UserProvider(IHttpContextAccessor httpContext) : IUserProvider
{
	public string? UserId => httpContext?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
