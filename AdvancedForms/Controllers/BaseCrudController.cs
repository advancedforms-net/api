using AdvancedForms.Helpers;
using AdvancedForms.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedForms.Controllers;

[Authorize]
[ApiController]
public abstract class BaseCrudeController<Model, CreateView, UpdateView> : ControllerBase where Model : class
{
	private readonly IBaseCrudService<Model, CreateView, UpdateView> crudService;
	protected readonly IFormService formService;
	protected readonly Guid userId;

	public BaseCrudeController(IBaseCrudService<Model, CreateView, UpdateView> crudService, IFormService formService, IHttpContextAccessor httpContextAccessor)
	{
		this.crudService = crudService;
		this.formService = formService;

		var userId = httpContextAccessor.HttpContext?.Items["UserId"] as Guid?;
		ArgumentNullException.ThrowIfNull(userId); // this should never happen because of the authorize attribute
		this.userId = userId.Value;
	}

	[HttpGet("{id}")]
	public async Task<Model> GetById(Guid id)
	{
		await ValidateUserAccess(id);
		return await crudService.Get(id);
	}

	/* Create not implemented due to differing return view and id from posted view
	[HttpPost]
	public async Task<Model> Create(CreateView view)
	{
		await ValidateUserAccess(id);

		return await crudService.Create(view);
	}*/

	[HttpPut("{id}")]
	public async Task<Model> Update(Guid id, UpdateView view)
	{
		await ValidateUserAccess(id);

		await crudService.Update(id, view);
		return await crudService.Get(id);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await ValidateUserAccess(id);
		await crudService.Delete(id);
		return Ok(new { message = $"{typeof(Model).Name} deleted" });
	}

	protected async Task ValidateUserAccess(Guid modelId)
	{
		Guid formId = await GetFormId(modelId);
		await ValidateUserAccessByFormId(formId);
	}
	protected async Task ValidateUserAccessByFormId(Guid formId)
	{
		await formService.ValidateUserAccess(formId, userId);
	}

	/// <summary>
	/// Get the form id from the model id
	/// </summary>
	/// <param name="modelId">Id of the model</param>
	/// <returns>Id of the form to which the model is linked</returns>
	protected abstract Task<Guid> GetFormId(Guid modelId);
}

