namespace AdvancedForms.Helpers;

public interface INowResolver
{
	DateTime GetNow();
	DateTime GetUtcNow();
}

public class NowResolver : INowResolver
{
	public DateTime GetNow()
	{
		return DateTime.Now;
	}

	public DateTime GetUtcNow()
	{
		return DateTime.UtcNow;
	}
}
