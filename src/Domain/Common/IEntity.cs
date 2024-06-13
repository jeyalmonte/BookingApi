namespace Domain.Common;

public interface IEntity
{
	List<IDomainEvent> PopDomainEvents();
}
