namespace AdvancedForms.Models;

/// <summary>
/// The response is used for the data send from a client.
/// </summary>
public class Response
{
	public Guid Id { get; set; }

	public DateTime Creation { get; set; }

	public Guid PresetId { get; set; }
	public Preset? Preset { get; set; }
	public List<ResponseValue> Values { get; } = new();
}

/// <summary>
/// Response values are theoreticly the value passed from the template but could be anything setup in the form
/// </summary>
public class ResponseValue
{
	public Guid Id { get; set; }

	public string Key { get; set; } = string.Empty;
	public string Value { get; set; } = string.Empty;

	public Guid ResponseId { get; set; }
	public Response? Response { get; set; }
}