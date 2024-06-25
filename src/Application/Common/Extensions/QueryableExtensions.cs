using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Extensions;

public static class QueryableExtensions
{
	public static async Task<PaginatedResult<TDestination>> PaginatedListAsync<TDestination>(
		this IQueryable<TDestination> queryable,
		int pageNumber,
		int pageSize,
		CancellationToken cancellationToken = default) where TDestination : class
	{
		return await PaginatedResult<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, cancellationToken);
	}
}
