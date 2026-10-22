public class Singleton<T> where T : class, new()
{
	private static T instance;

	public static T I => null;

	public virtual void Init()
	{
	}
}
