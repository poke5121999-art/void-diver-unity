using System;
using System.IO;
using ProtoBuf.Meta;
using SomeNet.TypeAlias;

namespace SomeNet.Message
{
	public class PacketSerializer
	{
		private struct StreamLengthMarker
		{
			private readonly Stream _stream;

			private readonly long _startPosition;

			private long _endPosition;

			public long StartPosition => 0L;

			public long EndPosition => 0L;

			public int Length => 0;

			public StreamLengthMarker(Stream stream, bool moveToNext)
			{
				_stream = null;
				_startPosition = 0L;
				_endPosition = 0L;
			}

			public void WriteLength(bool recoverStreamPosition)
			{
			}
		}

		private struct EncryptContext
		{
			public int Key;

			public int Hash;

			public int Index;
		}

		private struct DecryptContext
		{
			public int Key;

			public int Hash;

			public int Index;
		}

		public readonly MessageSerializer _messageSerializer;

		public readonly TypeAliasTable _typeTable;

		private int _serializeWrapKey;

		private int _serializeWrapPendingKey;

		private int _deserializeWrapKey;

		public PacketSerializer(TypeModel typeModel, TypeAliasTable typeTable)
		{
		}

		public void SetSerializeWrapKey(int wrapKey, bool pending = false)
		{
		}

		public void SetDeserializeWrapKey(int wrapKey)
		{
		}

		public int EstimateLength(object packet)
		{
			return 0;
		}

		public void Serialize(Stream stream, object packet)
		{
		}

		public int PeekLength(Stream stream)
		{
			return 0;
		}

		public object Deserialize(Stream stream)
		{
			return null;
		}

		public ParseHeaderResult TryParseHeader(Stream stream, out Type type, out int messageLen, out object extraInfo)
		{
			type = null;
			messageLen = default;
			extraInfo = null;
			return (ParseHeaderResult)0;
		}

		protected void GetBuffers(Stream stream, int pos, int length, out ArraySegment<byte> segment0, out ArraySegment<byte> segment1)
		{
			segment0 = default;
			segment1 = default;
		}

		private static void Encrypt(byte[] target, int targetOffset, byte[] source, int sourceOffset, int length, ref EncryptContext ctx)
		{
		}

		private static void Decrypt(byte[] target, int targetOffset, byte[] source, int sourceOffset, int length, ref DecryptContext ctx)
		{
		}
	}
}
