using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.Services;
using AdvancedForms.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedForms.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TemplateController : BaseCrudeController<PresetTemplate, PresetTemplateCreate, PresetTemplateUpdate>
{
	private readonly ITemplateService templateService;

	public TemplateController(ITemplateService templateService, IFormService formService, IHttpContextAccessor httpContextAccessor) :
		base(templateService, formService, httpContextAccessor)
	{
		this.templateService = templateService;
	}

	[HttpPost]
	public async Task<PresetTemplate> Create(PresetTemplateCreate model)
	{
		await ValidateUserAccessByFormId(model.FormId);
		return await templateService.Create(model);
	}

	protected override async Task<Guid> GetFormId(Guid modelId)
	{
		var template = await templateService.Get(modelId);
		return template.FormId;
	}
}

