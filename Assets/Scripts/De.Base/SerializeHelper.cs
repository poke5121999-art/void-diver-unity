using System;
using System.IO;
using ProtoBuf;

public static class SerializeHelper
{
	[ThreadStatic]
	private static MemoryStream _serializeStream;

	// SerializeHelper.Serialize<object> @0x180db4380 (machine code checked): the thread-static stream is created with capacity 512
	public static byte[] Serialize<T>(T instance)
	{
		if (_serializeStream == null)
		{
			_serializeStream = new MemoryStream(512);
		}
		_serializeStream.SetLength(0L);
		Serializer.Serialize(_serializeStream, instance);
		return _serializeStream.ToArray();
	}

	// SerializeHelper.Deserialize<object> @0x180db40f0 (machine code checked): the MemoryStream is disposed after reading
	public static T Deserialize<T>(byte[] bytes)
	{
		using (MemoryStream source = new MemoryStream(bytes))
		{
			return Serializer.Deserialize<T>(source);
		}
	}
}
