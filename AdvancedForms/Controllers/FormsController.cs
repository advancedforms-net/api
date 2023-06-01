using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.Services;
using AdvancedForms.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedForms.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FormsController : BaseCrudeController<Form, FormCreate, FormUpdate>
{
	public FormsController(ILogger<FormsController> logger, IFormService formService, IHttpContextAccessor httpContextAccessor):
		base(formService, formService, httpContextAccessor)
	{
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

	//TODO form export data
	//TODO preset import for codes
	// import csv witch desc and template link and generate a code preset for each
}
