using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.ViewModels;

namespace AdvancedForms.Services;

public interface IFormService
{
	Task<IEnumerable<Form>> GetAll();
	Task<Form> Get(Guid id);
	Task Create(FormCreate model);
	Task Update(Guid id, FormUpdate model);
	Task Delete(Guid id);
}

public class FormService : IFormService
{
	private readonly ILogger<FormService> logger;
	private readonly FormContext db;

	public FormService(ILogger<FormService> logger, FormContext db)
	{
		this.db = db;
		this.logger = logger;
	}
	public Task<IEnumerable<Form>> GetAll()
	{
		return Task.FromResult<IEnumerable<Form>>(db.Forms);
	}

	public async Task<Form> Get(Guid id)
	{
		var form = await db.Forms.FindAsync(id);
		if (form == null)
		{
			throw new KeyNotFoundException("Form not found");
		}

		return form;
	}

	public async Task Create(FormCreate model)
	{
		// validate
		//if (_context.Users.Any(x => x.Email == model.Email))
		//	throw new AppException("User with the email '" + model.Email + "' already exists");

		// map model to new user object
		var mapper = new Mapper();
		var form = mapper.FormCreateToForm(model);

		// save user
		db.Forms.Add(form);
		await db.SaveChangesAsync();
	}

	public async Task Update(Guid id, FormUpdate model)
	{
		var form = await Get(id);

		// validate
		//if (model.Email != form.Email && db.Users.Any(x => x.Email == model.Email))
		//	throw new AppException("User with the email '" + model.Email + "' already exists");

		// copy model to user and save
		var mapper = new Mapper();
		mapper.FormUpdateToForm(model, form);
		db.Forms.Update(form);
		await db.SaveChangesAsync();
	}

	public async Task Delete(Guid id)
	{
		var form = await Get(id);
		db.Forms.Remove(form);
		//TODO remove childs
		await db.SaveChangesAsync();
	}
}
