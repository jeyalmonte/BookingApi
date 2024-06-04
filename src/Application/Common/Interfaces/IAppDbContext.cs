using Domain.People;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
	DbSet<Person> People { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


