namespace AdvancedForms;

public interface INowResolver { DateTime GetNow(); }

public class NowResolver : INowResolver
{
	public DateTime GetNow()
	{
		return DateTime.Now;
	}
}
