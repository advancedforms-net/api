using AdvancedForms.Services;

namespace AdvancedForms.Helpers;

public class JwtMiddleware
{
	private readonly RequestDelegate next;

	public JwtMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	public async Task Invoke(HttpContext context, IUserService userService)
	{
		var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		if (token != null)
		{
			// attach user to context on successful jwt validation
			context.Items["UserId"] = await userService.ParseToken(token);
		}

		await next(context);
	}
}
