using AdvancedForms;
using AdvancedForms.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var provider = configuration.GetValue("Provider", "Sqlite");
var providerType = DbProviderType();

// Add services to the container.
builder.Services.AddTransient<INowResolver, NowResolver>();
builder.Services.AddDbContext<FormContext>(options => DbOptionsBuilder(options));

_ = provider switch
{
	"Sqlite" => builder.Services.AddDbContext<SqliteFormContext>(options => DbOptionsBuilder(options)),
	"Postgres" => builder.Services.AddDbContext<NpgsqlFormContext>(options => DbOptionsBuilder(options)),
	_ => throw new Exception($"Unsupported provider: {provider}"),
};

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	DbMigration(scope.ServiceProvider);
}

app.Run();

void DbMigration(IServiceProvider services)
{
	var providerType = DbProviderType();
	using var db = (FormContext)services.GetRequiredService(providerType);
	db.Database.Migrate();

	if (app.Environment.IsDevelopment())
	{
		// only seed the database when in development and the forms are empty
		if (db.Forms.Count() > 0)
			return;

		DataSeeder.Init(10);

		db.Forms.AddRange(DataSeeder.Forms);
		db.Presets.AddRange(DataSeeder.Presets);

		db.SaveChanges();
	}
}

DbContextOptionsBuilder DbOptionsBuilder(DbContextOptionsBuilder options)
{
	return provider switch
	{
		"Sqlite" => options.UseSqlite(configuration.GetConnectionString("db")),
		"Postgres" => options.UseNpgsql(configuration.GetConnectionString("db")),

		_ => throw new Exception($"Unsupported provider: {provider}"),
	};
}
Type DbProviderType()
{
	return provider switch
	{
		"Sqlite" => typeof(SqliteFormContext),
		"Postgres" => typeof(NpgsqlFormContext),

		_ => throw new Exception($"Unsupported provider: {provider}"),
	};
}
