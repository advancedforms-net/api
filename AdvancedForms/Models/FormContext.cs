using Microsoft.EntityFrameworkCore;

namespace AdvancedForms.Models;

public class FormContext : DbContext
{
	public DbSet<Form> Forms => Set<Form>();
	public DbSet<Preset> Presets => Set<Preset>();
	public DbSet<Response> Responses => Set<Response>();

	public FormContext() { }
	public FormContext(DbContextOptions<FormContext> options)
		: base(options)
	{
	}
	protected FormContext(DbContextOptions options) : base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();
}

public class SqliteFormContext : FormContext
{
	public SqliteFormContext() { }
	public SqliteFormContext(DbContextOptions<SqliteFormContext> options) : base(options)
	{
	}

#if EF_MIGRATION
	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		options.UseSqlite("DataSource=");
	}
#endif
}

public class NpgsqlFormContext : FormContext
{
	public NpgsqlFormContext() { }
	public NpgsqlFormContext(DbContextOptions<NpgsqlFormContext> options) : base(options)
	{
	}

#if EF_MIGRATION
	protected override void OnConfiguring(DbContextOptionsBuilder options) {
		options.UseNpgsql("DataSource=");
	}
#endif
}
