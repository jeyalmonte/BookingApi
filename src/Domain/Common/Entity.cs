namespace Domain.Common;

public abstract class Entity<TId> : IEntity
	where TId : struct
{
	public TId Id { get; private init; }
	protected Entity() { }
	protected Entity(TId id) => Id = id;

	private readonly List<IDomainEvent> _domainEvents = [];
	protected void RaiseEvent(IDomainEvent domainEvent)
		=> _domainEvents.Add(domainEvent);
	public List<IDomainEvent> PopDomainEvents()
	{
		var copy = _domainEvents.ToList();
		_domainEvents.Clear();

		return copy;
	}

}
