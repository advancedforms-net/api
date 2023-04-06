namespace AdvancedForms.Models;

public class Form
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public bool UseCodes { get; set; }

	public List<Preset> Presets { get; } = new();

	public List<Response> Responses { get; } = new();
}
