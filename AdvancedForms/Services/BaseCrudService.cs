using AdvancedForms.Models;

namespace AdvancedForms.Services;

public interface IBaseCrudService<Model, CreateView, UpdateView> where Model: class
{
	Task<Model> Get(Guid id);
	Task<Model> Create(CreateView view);
	Task Update(Guid id, UpdateView view);
	Task Delete(Guid id);
}

public abstract class BaseCrudService<Model, CreateView, UpdateView> : IBaseCrudService<Model, CreateView, UpdateView> where Model : class
{
	protected readonly FormContext db;

	public BaseCrudService(FormContext db)
	{
		this.db = db;
	}

	public virtual async Task<Model> Get(Guid id)
	{
		var model = await db.Set<Model>().FindAsync(id);
		if (model == null)
		{
			throw new KeyNotFoundException($"{typeof(Model).Name} not found");
		}

		return model;
	}

	public virtual async Task<Model> Create(CreateView view)
	{
		// map model to new db object
		var model = ViewToModel(view);

		// save
		db.Set<Model>().Add(model);
		await db.SaveChangesAsync();

		return model;
	}

	public virtual async Task Update(Guid id, UpdateView view)
	{
		var model = await Get(id);

		// copy model to db and save
		ViewToModel(view, model);

		db.Set<Model>().Update(model);
		await db.SaveChangesAsync();
	}

	public virtual async Task Delete(Guid id)
	{
		var model = await Get(id);

		DeleteActions(model);
		db.Set<Model>().Remove(model);

		await db.SaveChangesAsync();
	}

	public abstract Model ViewToModel(CreateView view);
	public abstract void ViewToModel(UpdateView view, Model model);

	public abstract void DeleteActions(Model model);
}
