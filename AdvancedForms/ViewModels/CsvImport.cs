using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace AdvancedForms.ViewModels;

public class CsvImportData
{
	public IFormFile? File { get; set; }

	public string TemplateMapJson{ get; set; } = default!;

	public Dictionary<string, Guid>? TemplateMap
	{
		get => JsonSerializer.Deserialize<Dictionary<string, Guid>>(TemplateMapJson) ?? new();
	}
}

public class CsvImport
{
	public string? Description { get; set; }
	public string? Code { get; set; }
	public string? TemplateKey { get; set; }
	public string? ValuesJson { get; set; }
}
