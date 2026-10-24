using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	// MonoSingleton<object>.get_I @0x1816ad600 (machine code checked): FindAnyObjectByType, else a new GameObject named after T with T added; DontDestroyOnLoad only on this first resolve
	public static T I
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindAnyObjectByType<T>();
				if (instance == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = typeof(T).Name;
					instance = gameObject.AddComponent<T>();
				}
				Object.DontDestroyOnLoad(instance);
			}
			return instance;
		}
	}
}
