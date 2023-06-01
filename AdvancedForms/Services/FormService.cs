using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AdvancedForms.Services;

public interface IFormService : IBaseCrudService<Form, FormCreate, FormUpdate>
{
	Task<IEnumerable<FormBasic>> GetAll(Guid? userId = null);

	Task<FormBasic> Create(FormCreate model, Guid userId);

	Task ValidateUserAccess(Guid formId, Guid userId);


	[Obsolete("Form create should be done with userid", true)]
	new Task<Form> Create(FormCreate view);
}

public class FormService : BaseCrudService<Form, FormCreate, FormUpdate>, IFormService
{
	private readonly Mapper mapper = new();

	public FormService(FormContext db): base(db)
	{
	}
	public async Task<IEnumerable<FormBasic>> GetAll(Guid? userId)
	{
		var forms = await db.Forms.Where(f => !userId.HasValue || f.UserId == userId).ToListAsync();
		return forms.Select(f => mapper.FormToFormBasic(f));
	}

	public override Task<Form> Create(FormCreate view)
	{
		throw new NotSupportedException("Form create should be done with userId");
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

	public async Task ValidateUserAccess(Guid formId, Guid userId)
	{
		var form = await Get(formId);

		if (form.UserId != userId)
		{
			throw new UnauthorizedException("Form not accessable by user.");
		}
	}

	public override Form ViewToModel(FormCreate view)
	{
		// TODO validate unique names?
		return mapper.FormCreateToForm(view);
	}

	public override void ViewToModel(FormUpdate view, Form model)
	{
		// TODO validate unique names?
		mapper.FormUpdateToForm(view, model);
	}

	public override void DeleteActions(Form model)
	{
		//TODO remove linked presets
		throw new NotImplementedException();
	}
}
