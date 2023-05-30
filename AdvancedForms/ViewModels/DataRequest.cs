namespace AdvancedForms.ViewModels;

public class DataRequest
{
	public Dictionary<string, string> StaticData { get; set; } = new();
	public Dictionary<string, string> ResponseData { get; set; } = new();
}
