using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.ViewModels;

namespace AdvancedForms.Services;

public interface IPresetService : IBaseCrudService<Preset, PresetCreate, PresetUpdate>
{
}

public class PresetService : BaseCrudService<Preset, PresetCreate, PresetUpdate>, IPresetService
{
	private readonly Mapper mapper = new();

	public PresetService(FormContext db) : base(db)
	{
	}

	public override Preset ViewToModel(PresetCreate view)
	{
 		return mapper.PresetCreateToPreset(view);
	}

	public override void ViewToModel(PresetUpdate view, Preset model)
	{
		mapper.PresetUpdateToPreset(view, model);
	}

	public override void DeleteActions(Preset model)
	{
		//TODO remove all linked responses
		throw new NotImplementedException();
	}
}
