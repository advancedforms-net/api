using AdvancedForms.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using AdvancedForms.Models;
using Microsoft.Extensions.Options;

namespace AdvancedForms.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
	private readonly IUserService userService;
	private readonly MailConfig mailConfig;
	public UsersController(IUserService userService, IOptions<MailConfig> mailConfig)
	{
		this.userService = userService;
		this.mailConfig = mailConfig.Value;
	}

	[HttpPost("Authenticate")]
	public async Task<ActionResult<string>> Authenticate(string mail)
	{
		string token = await userService.Authenticate(mail);

		//TODO mail the token
		/*var subject = mailConfig.Subject ?? "AdvancedForms login";
		var body = mailConfig.Body ?? "Login using following url: http://afui.net/login?jwt={token}";

		var client = new SmtpClient(mailConfig.Host, mailConfig.Port)
		{
			Credentials = new NetworkCredential(mailConfig.Username, mailConfig.Password),
			EnableSsl = mailConfig.UseSsl,
		};

		client.Send(mailConfig.Sender, mail, subject, body);*/

		return Ok(token);
	}
}
