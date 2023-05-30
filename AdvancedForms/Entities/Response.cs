using AdvancedForms.Entities;
using System.Text.Json.Serialization;

namespace AdvancedForms.Models;

/// <summary>
/// The response is used for the data send from a client.
/// </summary>
public class Response: ValuesModel
{
	public Guid Id { get; set; }

	public DateTime Creation { get; set; }

	public Guid PresetId { get; set; }

	[JsonIgnore]
	public virtual Preset Preset { get; set; } = default!;
}
