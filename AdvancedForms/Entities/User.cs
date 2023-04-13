namespace AdvancedForms.Models;

public class User
{
	public Guid Id { get; set; }
	public string Mail { get; set; } = string.Empty;

	public virtual List<Form> Forms { get; set; } = new();
}
