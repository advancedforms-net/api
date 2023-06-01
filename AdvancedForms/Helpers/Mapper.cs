using AdvancedForms.Models;
using AdvancedForms.ViewModels;
using Riok.Mapperly.Abstractions;

namespace AdvancedForms.Helpers;

[Mapper]
public partial class Mapper
{
	public partial FormBasic FormToFormBasic(Form form);
	public partial Form FormCreateToForm(FormCreate form);
	public partial void FormUpdateToForm(FormUpdate formUpdate, Form form);

	public partial PresetTemplate PresetTemplateCreateToPresetTemplate(PresetTemplateCreate template);
	public partial void PresetTemplateUpdateToPresetTemplate(PresetTemplateUpdate templateUpdate, PresetTemplate template);

	public partial Preset PresetCreateToPreset(PresetCreate template);
	public partial void PresetUpdateToPreset(PresetUpdate templateUpdate, Preset template);
}
