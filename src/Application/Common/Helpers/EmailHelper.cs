namespace Application.Common.Helpers
{
	public static class EmailHelper
	{
		public static string GetTemplate(string template, Dictionary<string, string> keyValues)
		{
			var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", $"{template}.html");
			var emailBody = File.ReadAllText(templatePath);
			return ReplaceProperties(emailBody, keyValues);
		}

		private static string ReplaceProperties(string emailBody, Dictionary<string, string> keyValues)
		{
			foreach (var (key, value) in keyValues)
			{
				emailBody = emailBody.Replace($"$[{key}]", value);
			}
			return emailBody;
		}
	}
}
