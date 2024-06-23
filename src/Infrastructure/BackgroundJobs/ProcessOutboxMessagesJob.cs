using Domain.Common;
using Infrastructure.Messaging.Outbox;
using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Infrastructure.BackgroundJobs;
[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(
	AppDbContext _dbContext,
	IPublisher _publisher) : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		var messages = await _dbContext
			.Set<OutboxMessage>()
			.Where(x => !x.Processed)
			.Take(20)
			.ToListAsync();

		foreach (var message in messages)
		{
			IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content,
				new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.All
				});

			if (domainEvent is null)
			{
				continue;
			}

			await _publisher.Publish(domainEvent, context.CancellationToken);
			message.MarkProcessed();
		}

		await _dbContext.SaveChangesAsync(context.CancellationToken);
	}
}