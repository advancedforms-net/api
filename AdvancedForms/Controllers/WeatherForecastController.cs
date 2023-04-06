using AdvancedForms.Models;
using AdvancedForms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedForms.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
	{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

		private readonly ILogger<WeatherForecastController> logger;
		private readonly FormContext db;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, FormContext db)
		{
			this.logger = logger;
			this.db = db;
		}

		[HttpGet("GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet("Data")]
		public async Task<ActionResult<DataRequest>> GetData(Guid formId, string? personalCode)
		{
			// check if the form is valid and if the form uses personal codes or not
			var form = await db.Forms.FindAsync(formId);
			if (form == null)
			{
				return NotFound();
			}

			if (form.UseCodes && string.IsNullOrEmpty(personalCode))
			{
				return BadRequest("Personal code is required for this form.");
			}

			if (!form.UseCodes)
			{
				// if the form does noet use codes then make sure the personal code is not filled in
				personalCode = null;
			}

			var preset = await db.Presets.Where(p => p.FormId == formId && p.Code == personalCode).SingleOrDefaultAsync();
			if (preset == null)
			{
				return NotFound();
			}

			var data = new DataRequest();

			// get data from preset template
			if (preset.Template != null)
			{
				data.StaticData = preset.Template.Values.ToDictionary(v => v.Key, v => v.Value);
			}

			// get already posted data (response data => personal code)
			if (form.UseCodes)
			{
				// get data from preset itself (only really relevant when using a personel code)
				preset.Values.ForEach(v => data.StaticData[v.Key] = v.Value);

				// get reponse data when codes are used
				var lastResponse = preset.Responses.OrderBy(r => r.Creation).FirstOrDefault();
				if (lastResponse != null)
				{
					data.ResponseData = lastResponse.Values.ToDictionary(v => v.Key, v => v.Value);
				}
			}

			return data;
		}

		[HttpPost("Data")]
		public async Task<ActionResult> PostData(Guid formId, string? personalCode, DataSubmit data)
		{
			ArgumentNullException.ThrowIfNull(data);

			// check if the form is valid and if the form uses personal codes or not
			var form = await db.Forms.FindAsync(formId);
			if (form == null)
			{
				return NotFound();
			}

			if (form.UseCodes && string.IsNullOrEmpty(personalCode))
			{
				return BadRequest("Personal code is required for this form.");
			}

			if (!form.UseCodes)
			{
				// if the form does noet use codes then make sure the personal code is not filled in
				personalCode = null;
			}

			var preset = await db.Presets.Where(p => p.FormId == formId && p.Code == personalCode).SingleOrDefaultAsync();
			if (preset == null)
			{
				return NotFound();
			}

			//TODO actually add data to db

			return NoContent();
		}
	}
}