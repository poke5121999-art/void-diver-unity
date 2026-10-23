using System;
using System.IO;
using ProtoBuf;

public static class SerializeHelper
{
	[ThreadStatic]
	private static MemoryStream _serializeStream;

	// TODO(decomp): generic method has no pseudo-C in the binary; reuse of the thread-static stream is assumed
	public static byte[] Serialize<T>(T instance)
	{
		if (_serializeStream == null)
		{
			_serializeStream = new MemoryStream();
		}
		_serializeStream.SetLength(0L);
		Serializer.Serialize(_serializeStream, instance);
		return _serializeStream.ToArray();
	}

	// TODO(decomp): generic method has no pseudo-C in the binary
	public static T Deserialize<T>(byte[] bytes)
	{
		return Serializer.Deserialize<T>(new MemoryStream(bytes));
	}
}
