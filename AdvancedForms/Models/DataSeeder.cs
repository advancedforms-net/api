using Bogus;

namespace AdvancedForms.Models;

public class DataSeeder
{
	public static List<Form> Forms = new List<Form>();
	public static List<Preset> Presets = new List<Preset>();

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

			DataSeeder.Presets.AddRange(presets);

#pragma warning disable CS8603 // Possible null reference return.
			return null; // Form.Presets is a getter only. The return value has no impact.
#pragma warning restore CS8603 // Possible null reference return.
		});

		var forms = formFaker.Generate(count);

		DataSeeder.Forms.AddRange(forms);
	}
}
