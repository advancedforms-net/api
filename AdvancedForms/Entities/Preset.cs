using AdvancedForms.Entities;
using System.Text.Json.Serialization;

namespace AdvancedForms.Models;

/// <summary>
/// Preset data is the date defined by the form creator to populate the form and labels. This data can not be edited by the person filling in the form.
/// </summary>
public class Preset: ValuesModel
{
	public Guid Id { get; set; }
	public string? Code { get; set; }

	public Guid FormId { get; set; }
	[JsonIgnore]
	public virtual Form Form { get; set; } = default!;

	public Guid? TemplateId { get; set; }
	public virtual PresetTemplate? Template { get; set; }

	public virtual List<Response> Responses { get; set; } = new();
}

/// <summary>
/// Preset template is used to share multiple common values over multiple presets.
/// </summary>
public class PresetTemplate: ValuesModel
{
	public Guid Id { get; set; }
	public string Description { get; set; } = string.Empty;

	// parent form under which this template falls
	[JsonIgnore] 
	public Guid FormId { get; set; }
	[JsonIgnore]
	public virtual Form Form { get; set; } = default!;

	// presets to which the template is linked
	[JsonIgnore]
	public virtual List<Preset> Presets { get; set; } = default!;
}
