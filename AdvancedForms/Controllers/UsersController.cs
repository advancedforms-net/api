using AdvancedForms.Services;
using Microsoft.AspNetCore.Mvc;
using AdvancedForms.Helpers;

namespace AdvancedForms.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
	private readonly IUserService userService;

	public UsersController(IUserService userService)
	{
		this.userService = userService;
	}

	[Authorize]
	[HttpGet("Validate")]
	public async Task<ActionResult<string>> Validate(string token)
	{
		// check db if there is a valid login
		return Ok("test");
	}

	[HttpPost("Authenticate")]
	public async Task<ActionResult<string>> Authenticate(string mail)
	{
		//TODO mail the token
		//TODO timespan from config
		return await userService.Authenticate(mail, TimeSpan.FromDays(3));
	}
}
