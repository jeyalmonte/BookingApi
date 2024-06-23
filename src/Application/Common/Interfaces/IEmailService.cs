namespace Application.Common.Interfaces;

public interface IEmailService
{
	Task<bool> SendAsync(string to, string subject, string content, CancellationToken cancellation = default);
}
