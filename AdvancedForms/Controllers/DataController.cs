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

		var preset = await GetFormPreset(formId, personalCode);
		if (preset == null)
		{
			return NotFound();
		}

		// actually add data to db
		preset.Responses.Add(new Response()
		{
			Creation = nowResolver.GetNow(),
			Values = data,
		});

		await db.SaveChangesAsync();

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
