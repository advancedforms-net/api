namespace AdvancedForms.Models;

/// <summary>
/// Preset data is the date defined by the form creator to populate the form and labels. This data can not be edited by the person filling in the form.
/// </summary>
public class Preset
{
	public Guid Id { get; set; }
	public string? Code { get; set; }

	public Guid FormId { get; set; }
	public virtual Form Form { get; set; } = default!;

	public Guid? TemplateId { get; set; }
	public virtual PresetTemplate? Template { get; set; }

	public virtual List<PresetValue> Values { get; set; } = new();
	public virtual List<Response> Responses { get; set; } = new();
}

/// <summary>
/// Preset template is used to share multiple common values over multiple presets.
/// </summary>
public class PresetTemplate
{
	public Guid Id { get; set; }
	public string Description { get; set; } = string.Empty;

	public virtual List<PresetTemplateValue> Values { get; set; } = new();
}

/// <summary>
/// Preset template values are used for creating the generic questions available for a template
/// e.g. common questions shared over multiple presets
/// </summary>
public class PresetTemplateValue
{
	public Guid Id { get; set; }

	public string Key { get; set; } = string.Empty;
	public string Value { get; set; } = string.Empty;

	public Guid TemplateId { get; set; }
	public virtual PresetTemplate Template { get; set; } = default!;
}

/// <summary>
/// Preset values contain personalized fields for the given code (e.g. name)
/// These can only be used when personalized codes are used.
/// </summary>
public class PresetValue
{
	public Guid Id { get; set; }

	public string Key { get; set; } = string.Empty;
	public string Value { get; set; } = string.Empty;

	public Guid PresetId { get; set; }
	public virtual Preset Preset { get; set; } = default!;
}

