using AdvancedForms.Models;
using AdvancedForms.ViewModels;
using Riok.Mapperly.Abstractions;

namespace AdvancedForms.Helpers;

[Mapper]
public partial class Mapper
{
	public partial Form FormCreateToForm(FormCreate form);
	public partial void FormUpdateToForm(FormUpdate formUpdate, Form form);
}
