using AdvancedForms.Controllers;
using Xunit;

namespace AdvancedForms.Tests;

public class UnitTest1
{
	[Fact]
	public void Test1()
	{
		var controller = new WeatherForecastController(null, null);
	}
}
