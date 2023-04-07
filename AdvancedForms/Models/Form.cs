namespace AdvancedForms.Models;

public class Form
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public bool UseCodes { get; set; }

	public virtual List<Preset> Presets { get; set; } = new();

	public virtual List<Response> Responses { get; set; } = new();
}
