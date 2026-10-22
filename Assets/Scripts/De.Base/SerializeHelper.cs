using System;
using System.IO;

public static class SerializeHelper
{
	[ThreadStatic]
	private static MemoryStream _serializeStream;

	public static byte[] Serialize<T>(T instance)
	{
		return null;
	}

	public static T Deserialize<T>(byte[] bytes)
	{
		return default;
	}
}
