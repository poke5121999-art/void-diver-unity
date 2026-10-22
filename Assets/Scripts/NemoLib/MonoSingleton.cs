using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	// TODO(decomp): generic shared code has no per-instantiation pseudo-C; lookup/creation shape assumed @0x0
	public static T I
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindFirstObjectByType<T>();
			}
			return instance;
		}
	}
}
