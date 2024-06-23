using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services;
public class SendgridEmailService(IConfiguration _configuration) : IEmailService
{
	private readonly SendGridClient _client = new(_configuration["Sendgrid:ApiKey"]);

	public async Task<bool> SendAsync(string to, string subject, string content, CancellationToken cancellation = default)
	{
		var msg = new SendGridMessage
		{
			From = new EmailAddress
			{
				Email = _configuration["Sendgrid:FromEmail"],
				Name = _configuration["Sendgrid:FromName"]
			},
			HtmlContent = content,
			Subject = subject,
		};

		msg.AddTo(new EmailAddress(to));

		var result = await _client.SendEmailAsync(msg, cancellation);

		return result.IsSuccessStatusCode;
	}
}
