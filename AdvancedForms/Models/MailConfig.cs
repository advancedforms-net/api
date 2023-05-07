namespace AdvancedForms.Models;

public class MailConfig
{
	public string Sender { get; set; } = default!;
	public string? Subject { get; set; }
	public string? Body { get; set; }

	public string Host { get; set; } = default!;
	public int Port { get; set; } = 25;

	public string? Username { get; set; }
	public string? Password { get; set; }

	public bool UseSsl { get; set; }
}
