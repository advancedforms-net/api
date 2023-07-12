using System.Globalization;
using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.Services;
using AdvancedForms.ViewModels;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedForms.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FormsController : BaseCrudeController<Form, FormCreate, FormUpdate>
{
	private readonly ILogger<FormsController> logger;
	public FormsController(ILogger<FormsController> logger, IFormService formService, IHttpContextAccessor httpContextAccessor):
		base(formService, formService, httpContextAccessor)
	{
		this.logger = logger;
	}

	[HttpGet]
	public async Task<IEnumerable<FormBasic>> GetAll()
	{
		return await formService.GetAll(userId);
	}

	[HttpPost]
	public async Task<FormBasic> Create(FormCreate model)
	{
		return await formService.Create(model, userId);
	}

	protected override Task<Guid> GetFormId(Guid modelId)
	{
		return Task.FromResult(modelId);
	}

	[HttpGet("{id}/Export")]
	public async Task<string> Export(Guid id)
	{
		await ValidateUserAccess(id);
		var form = await formService.Get(id);

		//TODO currently only supports csv format

		var records = form.Presets.SelectMany(p => p.Responses.Select(r => new CsvExport() {
			Description = p.Description,
			Code = p.Code,
			Creation = r.Creation,
			ValuesJson = r.ValuesJson,
		}));

		using var textWriter = new StringWriter();
		using var csv = new CsvWriter(textWriter, CultureInfo.InvariantCulture);
		csv.Context.RegisterClassMap<CsvExportMap>();

		await csv.WriteRecordsAsync(records);
		await csv.FlushAsync();

		return textWriter.ToString();
	}

	[HttpPost("{id}/Import")]
	public async Task Import(Guid id, [FromForm] CsvImportData importData)
	{
		if(importData.File == null)
		{
			throw new AppException("File should be provided for import.");

		}

		// import csv witch desc and template link and generate a code preset for each
		await ValidateUserAccess(id);

		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = true,
		};

		using var reader = new StreamReader(importData.File.OpenReadStream());
		using var csv = new CsvReader(reader, config);

		var records = csv.GetRecords<CsvImport>();

		//import class to form preset
		var presets = records.Select(r => new Preset() {
			FormId = id,
			Id = Guid.NewGuid(),
			Code = r.Code,
			Description = r.Description ?? string.Empty,
			ValuesJson = r.ValuesJson ?? string.Empty,
			TemplateId = TemplateIdFromKey(r.TemplateKey, importData.TemplateMap),
		});

		await formService.AddPresets(id, presets);
	}

	private Guid? TemplateIdFromKey(string? key, Dictionary<string, Guid>? templateMap)
	{
		if (string.IsNullOrEmpty(key) || templateMap == null)
		{
			return null;
		}

		if (templateMap.TryGetValue(key, out var id))
		{
			return id;
		}

		throw new AppException("Trying to use unmapped template ({0}).", key);
	}
}
