using Domain.Common;
using Infrastructure.Messaging.Outbox;
using Infrastructure.Persistence.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace Infrastructure.BackgroundJobs;
[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(
	AppDbContext _dbContext,
	IBus _publisher,
	ILogger<ProcessOutboxMessagesJob> _logger
	) : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		var messages = await _dbContext
			.Set<OutboxMessage>()
			.Where(x => !x.Processed)
			.OrderBy(x => x.CreatedAt)
			.Take(20)
			.ToListAsync(context.CancellationToken);

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

			await ProcessMessage(message, domainEvent, context);
		}

		await _dbContext.SaveChangesAsync(context.CancellationToken);
	}

	/// <summary>
	/// Publishes the domain event and marks the message as processed.
	/// if the message broker is down, the message will be retried on the next execution.
	/// </summary>
	/// <param name="message"></param>
	/// <param name="domainEvent"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	private async Task ProcessMessage(OutboxMessage message, IDomainEvent domainEvent, IJobExecutionContext context)
	{
		try
		{
			await _publisher.Publish(domainEvent, domainEvent.GetType(), context.CancellationToken);
			message.MarkProcessed();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to publish domain event: {Message}", ex.Message);
		}
	}
}