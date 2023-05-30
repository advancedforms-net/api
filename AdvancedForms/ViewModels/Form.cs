using System.ComponentModel.DataAnnotations;

namespace AdvancedForms.ViewModels;

public class FormCreate
{
	[Required]
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool UseCodes { get; set; }
}

public class FormUpdate
{
	public string? Name { get; set; }
	public string? Description { get; set; }
	public bool? UseCodes { get; set; }
}

public class FormBasic
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool UseCodes { get; set; }
}
