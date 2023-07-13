using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedForms.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{
	private readonly ILogger<DataController> logger;
	private readonly FormContext db;
	private readonly INowResolver nowResolver;

	public DataController(ILogger<DataController> logger, FormContext db, INowResolver nowResolver)
	{
		this.logger = logger;
		this.db = db;
		this.nowResolver = nowResolver;
	}

	[HttpGet("{formId}")]
	public async Task<ActionResult<DataRequest>> GetData(Guid formId, string? personalCode)
	{
		var preset = await GetFormPreset(formId, personalCode);
		if (preset == null)
		{
			return NotFound();
		}

		var data = new DataRequest();

		// get data from preset template
		if (preset.Template != null)
		{
			foreach (var v in preset.Template.Values)
			{
				data.StaticData[v.Key] = v.Value;
			}
		}

		// get data from preset itself (only really relevant when using a personel code)
		foreach (var v in preset.Values)
		{
			data.StaticData[v.Key] = v.Value;
		}

		// get already posted data (response data => personal code)
		if (preset.Form.UseCodes)
		{
			// get reponse data when codes are used
			var lastResponse = preset.Responses.OrderBy(r => r.Creation).FirstOrDefault();
			if (lastResponse != null)
			{
				data.ResponseData = lastResponse.Values;
			}
		}

		return data;
	}

	[HttpPost("{formId}")]
	public async Task<ActionResult> PostData(Guid formId, string? personalCode, Dictionary<string, string> data)
	{
		if (data == null)
		{
			return BadRequest("No data supplied.");
		}

		if (string.IsNullOrEmpty(personalCode))
		{
			// if the personalcode is not filled in the check the submitted data to see if it's was added there
			data.TryGetValue(nameof(personalCode), out personalCode);
			data.Remove(nameof(personalCode));
		}

		logger.LogInformation("Post data with code {personalCode}", personalCode);
		logger.LogInformation("Data: {data}", System.Text.Json.JsonSerializer.Serialize(data));

		var preset = await GetFormPreset(formId, personalCode);
		if (preset == null)
		{
			return NotFound();
		}

		// actually add data to db
		// we filter out all keys starting with _ as those are used by the system
		var responseData = data.Where(it => !it.Key.StartsWith('_')).ToDictionary(it => it.Key, it => it.Value);
		preset.Responses.Add(new Response()
		{
			Creation = nowResolver.GetNow(),
			Values = responseData,
		});

		await db.SaveChangesAsync();

		if (data.TryGetValue("_redirect", out var redirect) && !string.IsNullOrWhiteSpace(redirect))
		{
			return Redirect(redirect);
		}

		//TODO add simple success UI
		return NoContent();
	}

	[NonAction]
	public async Task<Preset?> GetFormPreset(Guid formId, string? personalCode)
	{
		// check if the form is valid and if the form uses personal codes or not
		var form = await db.Forms.FindAsync(formId);
		if (form == null)
		{
			return null;
		}

		if (form.UseCodes && string.IsNullOrEmpty(personalCode))
		{
			return null;
		}

		if (!form.UseCodes)
		{
			// if the form does noet use codes then make sure the personal code is not filled in
			personalCode = null;
		}

		var preset = await db.Presets.Where(p => p.FormId == formId && p.Code == personalCode).SingleOrDefaultAsync();

		return preset;
	}
}
