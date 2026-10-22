public class Singleton<T> where T : class, new()
{
	private static T instance;

	// TODO(decomp): generic shared code has no per-instantiation pseudo-C; lazy creation is the standard shape @0x0
	public static T I
	{
		get
		{
			if (instance == null)
			{
				instance = new T();
			}
			return instance;
		}
	}

	public virtual void Init()
	{
	}
}
