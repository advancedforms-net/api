using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AdvancedForms.Services;

public interface IFormService
{
	Task<IEnumerable<FormBasic>> GetAll(Guid? userId = null);
	Task<Form> Get(Guid formId);
	Task<FormBasic> Create(FormCreate model, Guid userId);
	Task Update(Guid formId, FormUpdate model);
	Task Delete(Guid formId);
}

public class FormService : IFormService
{
	private readonly ILogger<FormService> logger;
	private readonly FormContext db;
	private readonly Mapper mapper = new();

	public FormService(ILogger<FormService> logger, FormContext db)
	{
		this.db = db;
		this.logger = logger;
	}
	public async Task<IEnumerable<FormBasic>> GetAll(Guid? userId)
	{
		var forms = await db.Forms.Where(f => !userId.HasValue || f.UserId == userId).ToListAsync();
		return forms.Select(f => mapper.FormToFormBasic(f));
	}

	public async Task<Form> Get(Guid formId)
	{
		var form = await db.Forms.FindAsync(formId);
		if (form == null)
		{
			throw new KeyNotFoundException("Form not found");
		}

		return form;
	}

	public async Task<FormBasic> Create(FormCreate model, Guid userId)
	{
		// validate
		//if (_context.Users.Any(x => x.Email == model.Email))
		//	throw new AppException("User with the email '" + model.Email + "' already exists");

		// map model to new form object
		var form = mapper.FormCreateToForm(model);

		form.UserId = userId;

		// save form
		db.Forms.Add(form);
		await db.SaveChangesAsync();

		return mapper.FormToFormBasic(form);
	}

	public async Task Update(Guid formId, FormUpdate model)
	{
		var form = await Get(formId);

		// validate
		//if (model.Email != form.Email && db.Users.Any(x => x.Email == model.Email))
		//	throw new AppException("User with the email '" + model.Email + "' already exists");

		// copy model to form and save
		mapper.FormUpdateToForm(model, form);

		db.Forms.Update(form);
		await db.SaveChangesAsync();
	}

	public async Task Delete(Guid formId)
	{
		var form = await Get(formId);
		db.Forms.Remove(form);
		//TODO remove childs
		await db.SaveChangesAsync();
	}
}
