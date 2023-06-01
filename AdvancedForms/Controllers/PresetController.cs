using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.Services;
using AdvancedForms.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedForms.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PresetController : BaseCrudeController<Preset, PresetCreate, PresetUpdate>
{
	private readonly IPresetService presetService;

	public PresetController(IPresetService presetService, IFormService formService, IHttpContextAccessor httpContextAccessor):
		base(presetService, formService, httpContextAccessor)
	{
		this.presetService = presetService;
	}

	[HttpPost]
	public async Task<Preset> Create(PresetCreate model)
	{
		await ValidateUserAccessByFormId(model.FormId);
		return await presetService.Create(model);
	}

	protected override async Task<Guid> GetFormId(Guid modelId)
	{
		var preset = await presetService.Get(modelId);
		return preset.FormId;
	}
}

