
using AdvancedForms.Helpers;
using AdvancedForms.Models;
using AdvancedForms.ViewModels;
using NUnit.Framework;

namespace AdvancedForms.Tests;

public class MapperTest
{
	[Test]
	public void FormUpdate()
	{
		var form = new Form()
		{
			Id = System.Guid.NewGuid(),
			Name = "test",
			Description = "desc",
			UseCodes = false,
		};

		var formUpdate = new FormUpdate()
		{
			Name = null,
			Description = "update",
			UseCodes = null,
		};

		// copy model to user and save
		var mapper = new Mapper();
		mapper.FormUpdateToForm(formUpdate, form);

		Assert.AreEqual("test", form.Name);
		Assert.AreEqual("update", form.Description);
		Assert.AreEqual(false, form.UseCodes);
	}
}
