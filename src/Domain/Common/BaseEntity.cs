namespace Domain.Common;

public abstract class BaseEntity
{
	public Guid Id { get; private init; }

	protected readonly List<IDomainEvent> _domainEvents = [];

	protected BaseEntity() { }
	protected BaseEntity(Guid id) => Id = id;

	public List<IDomainEvent> PopDomainEvents()
	{
		var copy = _domainEvents.ToList();
		_domainEvents.Clear();

		return copy;
	}

}
