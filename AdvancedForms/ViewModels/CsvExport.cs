using CsvHelper.Configuration;

namespace AdvancedForms.ViewModels;

public class CsvExport
{
	public string? Description {get; set; }
	public string? Code { get; set; }
	public DateTime Creation { get; set; }

	//TODO format response value differently then json
	public string? ValuesJson { get; set; }
}
public class CsvExportMap : ClassMap<CsvExport>
{
	public CsvExportMap()
	{
		Map(m => m.Description).Index(0).Name("Description");
		Map(m => m.Code).Index(1).Name("Code");
		Map(m => m.Creation).Index(2).Name("Creation");
		Map(m => m.ValuesJson).Index(3).Name("Values");
	}
}