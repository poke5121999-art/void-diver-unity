public class Singleton<T> where T : class, new()
{
	private static T instance;

	// Singleton<object>.get_I @0x181a73fe0 (machine code checked): lazy Activator.CreateInstance<T>()
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
