namespace AdvancedForms.ViewModels;

public class DataRequest
{
	public Dictionary<string, string> StaticData { get; set; } = new Dictionary<string, string>();
	public Dictionary<string, string> ResponseData { get; set; } = new Dictionary<string, string>();
}
