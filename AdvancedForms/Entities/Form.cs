namespace AdvancedForms.Models;

public class Form
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public bool UseCodes { get; set; }

	// owner of the form
	public Guid UserId { get; set; }

	public virtual List<Preset> Presets { get; set; } = new();

	public virtual List<PresetTemplate> PresetTemplates { get; set; } = new();
}
