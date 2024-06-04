using Domain.People.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.People.Events
{
	public class PersonCreatedEventHandler(ILogger<PersonCreatedEvent> _logger) : INotificationHandler<PersonCreatedEvent>
	{
		public Task Handle(PersonCreatedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Person created: {Person}", notification.Person.Id);
			return Task.CompletedTask;
		}
	}
}
