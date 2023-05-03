using AdvancedForms.Helpers;
using AdvancedForms.Services;
using AdvancedForms.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedForms.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FormsController : ControllerBase
{
	private readonly ILogger<FormsController> logger;
	private readonly IFormService formService;
	private readonly Guid userId;

	public FormsController(ILogger<FormsController> logger, IFormService formService, IHttpContextAccessor httpContextAccessor)
	{
		this.logger = logger;
		this.formService = formService;

		var userId = httpContextAccessor.HttpContext?.Items["UserId"] as Guid?;
		ArgumentNullException.ThrowIfNull(userId); // this should never happen because of the authorize attribute
		this.userId = userId.Value;
	}

	/* endpoints
	form CRUD
	- export data
	form template CRUD
	form preset with code CRUD
	- responses are displayed on preset
	- export data
	*/

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var forms = await formService.GetAll();
		return Ok(forms);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		await ValidateUserAccess(id);
		var form = await formService.Get(id);
		return Ok(form);
	}

	[HttpPost]
	public async Task<IActionResult> Create(FormCreate model)
	{
		await formService.Create(model, userId);
		return Ok(new { message = "Form created" });
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(Guid id, FormUpdate model)
	{
		await ValidateUserAccess(id);
		await formService.Update(id, model);
		return Ok(new { message = "Form updated" });
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await ValidateUserAccess(id);
		await formService.Delete(id);
		return Ok(new { message = "Form deleted" });
	}

	private async Task ValidateUserAccess(Guid formId)
	{
		var form = await formService.Get(formId);

		if(form.UserId != userId)
		{
			throw new UnauthorizedException("Form not accessable by user.");
		}
	}
}
