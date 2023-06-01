using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.ViewModels;

namespace AdvancedForms.Services;

public interface ITemplateService : IBaseCrudService<PresetTemplate, PresetTemplateCreate, PresetTemplateUpdate>
{
}

public class TemplateService : BaseCrudService<PresetTemplate, PresetTemplateCreate, PresetTemplateUpdate>, ITemplateService
{
	private readonly Mapper mapper = new();

	public TemplateService(FormContext db): base(db)
	{
	}

	public override PresetTemplate ViewToModel(PresetTemplateCreate view)
	{
		return mapper.PresetTemplateCreateToPresetTemplate(view);
	}

	public override void ViewToModel(PresetTemplateUpdate view, PresetTemplate model)
	{
		mapper.PresetTemplateUpdateToPresetTemplate(view, model);
	}

	public override void DeleteActions(PresetTemplate model)
	{
		//TODO remove links to presets
		throw new NotImplementedException();
	}
}
