namespace Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
	public Guid Id { get; private set; }
	public string Type { get; private set; } = null!;
	public string Content { get; private set; } = null!;
	public DateTime CreatedAt { get; private set; }
	public bool Processed { get; private set; }
	public DateTime? ProcessedAt { get; private set; }

	public OutboxMessage(string messageType, string body)
	{
		Id = Guid.NewGuid();
		Type = messageType;
		Content = body;
		CreatedAt = DateTime.UtcNow;
		Processed = false;
	}

	public void MarkProcessed()
	{
		Processed = true;
		ProcessedAt = DateTime.UtcNow;
	}

	private OutboxMessage()
	{
	}
}
