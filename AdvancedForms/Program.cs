using AdvancedForms.Helpers;
using AdvancedForms.Services;
using AdvancedForms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var provider = configuration.GetValue("Provider", "Sqlite");
var providerType = DbProviderType();

// Add services to the container.
{
	var services = builder.Services;

	services.AddHealthChecks();
	services.AddHttpContextAccessor();

	services.AddTransient<INowResolver, NowResolver>();
	services.AddDbContext<FormContext>(options => DbOptionsBuilder(options));

	// configure strongly typed settings object
	services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
	services.Configure<MailConfig>(builder.Configuration.GetSection("Mail"));

	// configure DI for application services
	services.AddScoped<IUserService, UserService>();
	services.AddScoped<IFormService, FormService>();
	services.AddScoped<ITemplateService, TemplateService>();
	services.AddScoped<IPresetService, PresetService>();

	_ = provider switch
	{
		"Sqlite" => services.AddDbContext<SqliteFormContext>(options => DbOptionsBuilder(options)),
		"Postgres" => services.AddDbContext<NpgsqlFormContext>(options => DbOptionsBuilder(options)),
		_ => throw new Exception($"Unsupported provider: {provider}"),
	};

	services.AddControllers(options =>
	{
		//TODO only apply on DataController
		options.InputFormatters.Add(new FormInputFormatter());
	});

	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	services.AddEndpointsApiExplorer();
	services.AddSwaggerGen(options =>
	{
		var securityScheme = new OpenApiSecurityScheme()
		{
			Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
			Name = "Authorization",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.Http,
			Scheme = "bearer",
			BearerFormat = "JWT",
		};

		options.OperationFilter<AddAuthHeaderOperationFilter>();
		options.AddSecurityDefinition("bearer", securityScheme);
	});
}

var app = builder.Build();

app.MapHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// global cors policy
app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

// custom jwt auth middleware
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

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

		DataSeeder.Init(2);

		db.Users.AddRange(DataSeeder.Users);

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
