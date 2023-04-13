using Microsoft.AspNetCore.Mvc.Filters;

namespace AdvancedForms.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		var userId = context.HttpContext.Items["UserId"] as Guid?;
		if (userId == null)
		{
			// not logged in
			throw new UnauthorizedException("Unauthorized");
		}
	}
}
