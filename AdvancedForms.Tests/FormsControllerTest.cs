using AdvancedForms.Controllers;
using AdvancedForms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedForms.Tests;

public class FormsControllerGetDataTest
{
	private readonly DbContextOptions<FormContext> dbContextOptions = new DbContextOptionsBuilder<FormContext>()
		.UseInMemoryDatabase(databaseName: "Forms")
	.Options;

	private FormsController controller = default!;

	[OneTimeSetUp]
	public void Setup()
	{
		SeedDb();

		controller = new FormsController(new NullLogger<FormsController>(), new FormContext(dbContextOptions), new NowResolver());
	}

	private void SeedDb()
	{
		PresetTemplate template = new()
		{
			Description = "Codes template",
			Values = new()
			{
				new() { Key = "test-include", Value = "test value template" },
				new() { Key = "test-override", Value = "override value template" },
			}
		};

		using var context = new FormContext(dbContextOptions);
		var forms = new List<Form>
		{
			new()
			{
				Id = new Guid("b1fa714f-fbba-44d1-8de8-171e4614e881"),
				Name = "Form 1",
				Description = "Form 1 desc",
				UseCodes = false,
				Presets = new()
				{
					new()
					{
						Id = new Guid("7a9186d8-69c0-4918-a072-e693ba6199be"),
						Values = new ()
						{
							new() { Key = "test-exlude", Value = "test value preset" },
						},
						Template = new()
						{
							Description = "No codes template",
							Values= new ()
							{
								new() { Key = "test-include", Value = "test value template" },
							}
						}
					},
				},
			},
			new()
			{
				Id = new Guid("542a47b5-28fe-4e6a-9f20-9b8c4e9b1b92"),
				Name = "Form 2",
				Description = "Form 2 desc using codes",
				UseCodes = true,
				Presets = new()
				{
					new()
					{
						Id = new Guid("8d2c9532-9d33-4d51-9fae-805e5d8b81d5"),
						Code = "test1",
						Values = new ()
						{
							new() { Id = Guid.NewGuid(), Key = "test", Value = "test value 1" },
						},
						Template = template,
					},
					new()
					{
						Id = new Guid("4b973e97-4f01-4ece-a7dc-be822965d3c3"),
						Code = "test2",
						Values = new ()
						{
							new() { Id = Guid.NewGuid(), Key = "test", Value = "test value 2" },
							new() { Id = Guid.NewGuid(), Key = "test-override", Value = "value overriden" },
						},
						Template = template,
					},
				},
			},
		};

		context.AddRange(forms);

		context.SaveChanges();
	}
	[Test]
	public async Task FormUnknown()
	{
		var result = await controller.GetData(new Guid("b1fa714f-0000-44d1-8de8-171e4614e881"), null);

		Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
	}

	[Test]
	public async Task FormWithoutCode()
	{
		var result = await controller.GetData(new Guid("b1fa714f-fbba-44d1-8de8-171e4614e881"), null);
		var data1 = result.GetObjectResult();

		Assert.That(data1?.StaticData, Does.Not.ContainKey("test-exlude"));
		Assert.That(data1?.StaticData, Does.ContainKey("test-include"));

		result = await controller.GetData(new Guid("b1fa714f-fbba-44d1-8de8-171e4614e881"), "Hello");
		var data2 = result.GetObjectResult();

		Assert.That(data1?.StaticData, Is.EquivalentTo(data2?.StaticData));
	}

	[Test]
	public async Task FormWithCodeMissing()
	{
		var result = await controller.GetData(new Guid("542a47b5-28fe-4e6a-9f20-9b8c4e9b1b92"), null);
		Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
	}

	[Test]
	public async Task FormWithUnknownCode()
	{
		var result = await controller.GetData(new Guid("542a47b5-28fe-4e6a-9f20-9b8c4e9b1b92"), "Unknown code");
		Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
	}

	[Test]
	public async Task FormWithCode()
	{
		var result1 = await controller.GetData(new Guid("542a47b5-28fe-4e6a-9f20-9b8c4e9b1b92"), "test1");
		var data1 = result1.GetObjectResult();

		Assert.That(data1?.StaticData, Does.ContainKey("test"));
		Assert.That(data1?.StaticData["test"], Is.EqualTo("test value 1"));

		Assert.That(data1?.StaticData, Does.ContainKey("test-override"));
		Assert.That(data1?.StaticData["test-override"], Is.EqualTo("override value template"));

		var result2 = await controller.GetData(new Guid("542a47b5-28fe-4e6a-9f20-9b8c4e9b1b92"), "test2");
		var data2 = result2.GetObjectResult();

		Assert.That(data2?.StaticData, Does.ContainKey("test"));
		Assert.That(data2?.StaticData["test"], Is.EqualTo("test value 2"));

		Assert.That(data2?.StaticData, Does.ContainKey("test-override"));
		Assert.That(data2?.StaticData["test-override"], Is.EqualTo("value overriden"));
	}
}
