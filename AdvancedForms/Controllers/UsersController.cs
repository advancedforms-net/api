using AdvancedForms.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using AdvancedForms.Models;
using Microsoft.Extensions.Options;

namespace AdvancedForms.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	private readonly IUserService userService;
	private readonly MailConfig mailConfig;
	private readonly ILogger<UsersController> logger;
	public UsersController(ILogger<UsersController> logger, IUserService userService, IOptions<MailConfig> mailConfig)
	{
		this.logger = logger;
		this.userService = userService;
		this.mailConfig = mailConfig.Value;
	}

	[HttpPost("Authenticate")]
	public async Task<ActionResult<string>> Authenticate(string mail)
	{
		string token = await userService.Authenticate(mail);

		//TODO mail the token
		var subject = mailConfig.Subject ?? "AdvancedForms login";
		var body = mailConfig.Body ?? "Login using following url: http://advancedforms.net/login?jwt={token}";
		body = body.Replace("{token}", token);

		using var client = new SmtpClient(mailConfig.Host, mailConfig.Port)
		{
			Credentials = new NetworkCredential(mailConfig.Username, mailConfig.Password),
			EnableSsl = mailConfig.UseSsl,
		};

		logger.LogInformation("Sending login mail to {mail}", mail);
		client.Send(mailConfig.Sender, mail, subject, body);

		return Ok("Login info is mailed");
	}
}
