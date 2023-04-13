using AdvancedForms.Models;
using AdvancedForms.Services;
using Microsoft.Extensions.Options;

namespace AdvancedForms.Helpers;

public class JwtMiddleware
{
	private readonly RequestDelegate _next;
	private readonly AppSettings _appSettings;

	public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
	{
		_next = next;
		_appSettings = appSettings.Value;
	}

	public async Task Invoke(HttpContext context, IUserService userService)
	{
		var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		if (token != null)
		{
			// attach user to context on successful jwt validation
			context.Items["UserId"] = await userService.ParseToken(token);
		}

		await _next(context);
	}
}
