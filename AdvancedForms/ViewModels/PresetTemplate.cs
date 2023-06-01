using System.ComponentModel.DataAnnotations;

namespace AdvancedForms.ViewModels;

public class PresetTemplateCreate
{
	[Required]
	public string Description { get; set; } = string.Empty;
	public Dictionary<string, string>? Values { get; set; }

	[Required]
	public Guid FormId { get; set; }
}

public class PresetTemplateUpdate
{
	public string? Description { get; set; }
	public Dictionary<string, string>? Values { get; set; }
}
