using Microsoft.AspNetCore.Mvc;

namespace AdvancedForms.Tests;

public static class ActionResultHelper
{
	public static T? GetObjectResult<T>(this ActionResult<T> result)
	{
		if (result.Result != null)
			return (T?)((ObjectResult)result.Result).Value;
		return result.Value;
	}
}
