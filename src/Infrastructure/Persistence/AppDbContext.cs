using Application.Common.Interfaces;
using Domain.Common;
using Domain.People;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(
	DbContextOptions options,
	IPublisher _publisher) : DbContext(options), IAppDbContext
{
	public DbSet<Person> People { get; set; } = null!;

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var domainEvents = ChangeTracker.Entries<BaseEntity>()
			.SelectMany(x => x.Entity.PopDomainEvents())
			.ToList();

		await PublishDomainEvents(domainEvents, cancellationToken);

		return await base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

		base.OnModelCreating(modelBuilder);
	}

	private async Task PublishDomainEvents(List<IDomainEvent> domainEvents, CancellationToken cancellationToken)
	{
		foreach (var domainEvent in domainEvents)
		{
			await _publisher.Publish(domainEvent, cancellationToken);
		}
	}
}
