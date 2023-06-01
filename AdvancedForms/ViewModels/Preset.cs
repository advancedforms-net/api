using AdvancedForms.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdvancedForms.ViewModels;

public class PresetCreate
{
	public string? Code { get; set; }
	[Required]
	public string Description { get; set; } = string.Empty;

	[Required]
	public Guid FormId { get; set; }

	public Guid? TemplateId { get; set; }

	public Dictionary<string, string>? Values { get; set; }
}

public class PresetUpdate
{
	public string? Code { get; set; }
	public string? Description { get; set; }

	public Guid? TemplateId { get; set; }

	public Dictionary<string, string>? Values { get; set; }
}
