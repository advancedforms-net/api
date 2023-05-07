namespace AdvancedForms.Models;

public class JwtConfig
{
	public int ExpireDays { get; set; } = 3;
	public string Secret { get; set; } = default!;
}
