using Microsoft.EntityFrameworkCore;

namespace AdvancedForms.Models;

public class FormContext : DbContext
{
	public DbSet<Form> Forms { get; set; }
	public DbSet<Preset> Presets { get; set; }
	public DbSet<Response> Responses { get; set; }

	public string DbPath { get; }

	public FormContext()
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		DbPath = System.IO.Path.Join(path, "forms.db");
	}

	// The following configures EF to create a Sqlite database file in the
	// special "local" folder for your platform.
	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseSqlite($"Data Source={DbPath}");
}
/*public class NpgsqlFormContext : FormContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseNpgsql("DataSource=");
}*/