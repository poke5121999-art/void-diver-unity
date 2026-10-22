using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
public static class AotInitializer
{
	[Preserve]
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void EnsureReferenced()
	{
	}

	[Preserve]
	public static void AotGenericInstantiationHints()
	{
	}

	[Preserve]
	private static void HintProtoBuf()
	{
	}

	[Preserve]
	private static void TouchProto<T>(MemoryStream stream)
	{
	}

	[Preserve]
	private static void HintNewtonsoftJson()
	{
	}

	[Preserve]
	private static void HintJsonUtility()
	{
	}

	[Preserve]
	private static void HintEnumTryParse()
	{
	}

	[Preserve]
	private static void HintCollections()
	{
	}

	[Preserve]
	private static void HintUniRx()
	{
	}

	[Preserve]
	private static void HintUniTask()
	{
	}

	[Preserve]
	private static void HintTrackable()
	{
	}
}
