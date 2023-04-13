using Bogus;

namespace AdvancedForms.Models;

public class DataSeeder
{
	public static List<User> Users = new List<User>();

	public static void Init(int count)
	{
		var presetFaker = new Faker<Preset>()
			.RuleFor(p => p.Id, f => f.Random.Guid());

		var formFaker = new Faker<Form>()
		.RuleFor(f => f.Id, f => f.Random.Guid())
		.RuleFor(f => f.Name, f => f.Lorem.Word())
		.RuleFor(f => f.Description, f => f.Hacker.Phrase())
		.RuleFor(f => f.UseCodes, f => f.Random.Bool())
		.RuleFor(f => f.Presets, (f, form) =>
		{
			presetFaker.RuleFor(p => p.FormId, _ => form.Id);
			presetFaker.RuleFor(p => p.Code, _ => form.UseCodes ? f.Random.String2(5) : null);

			var presets = form.UseCodes ? presetFaker.GenerateBetween(3, 15) : presetFaker.GenerateBetween(1, 1);

			return presets;
		});

		var userFaker = new Faker<User>()
			.RuleFor(u => u.Id, f => f.Random.Guid())
			.RuleFor(u => u.Mail, f => f.Person.Email)
			.RuleFor(u => u.Forms, (f, user) =>
			{
				formFaker.RuleFor(f => f.UserId, _ => user.Id);

				var forms = formFaker.GenerateBetween(1, 5);
				return forms;
			});

		var users = userFaker.Generate(count);

		DataSeeder.Users.AddRange(users);
	}
}
