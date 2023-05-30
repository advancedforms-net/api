using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdvancedForms.Entities;

public class ValuesModel
{   /// <summary>
	/// Preset template values are used for creating the generic questions available for a template
	/// e.g. common questions shared over multiple presets
	/// </summary>
	[NotMapped]
	public Dictionary<string, string> Values { get; set; } = new();

	[JsonIgnore]
	public string ValuesJson
	{
		get => JsonSerializer.Serialize(Values);
		set => Values = JsonSerializer.Deserialize<Dictionary<string, string>>(value) ?? new();
	}
}
