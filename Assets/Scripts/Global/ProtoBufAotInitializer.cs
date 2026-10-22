using ProtoBuf.Meta;
using UnityEngine;

public static class ProtoBufAotInitializer
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
	}

	private static void Prewarm(RuntimeTypeModel model)
	{
	}
}
