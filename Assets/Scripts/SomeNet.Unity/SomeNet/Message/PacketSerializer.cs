using System;
using System.IO;
using ProtoBuf.Meta;
using SomeNet.TypeAlias;
using SomeNet.Utility;

namespace SomeNet.Message
{
	public class PacketSerializer
	{
		private struct StreamLengthMarker
		{
			private readonly Stream _stream;

			private readonly long _startPosition;

			private long _endPosition;

			public long StartPosition => _startPosition;

			public long EndPosition => _endPosition;

			public int Length => (int)_endPosition - (int)_startPosition - 4;

			public StreamLengthMarker(Stream stream, bool moveToNext)
			{
				_stream = stream;
				_startPosition = stream.Position;
				_endPosition = 0L;
				if (moveToNext)
				{
					stream.Seek(4L, SeekOrigin.Current);
				}
			}

			public void WriteLength(bool recoverStreamPosition)
			{
				_endPosition = _stream.Position;
				int length = (int)_endPosition - (int)_startPosition;
				_stream.Seek(_startPosition, SeekOrigin.Begin);
				byte[] bytes = BitConverter.GetBytes(length - 4);
				_stream.Write(bytes, 0, bytes.Length);
				if (recoverStreamPosition)
				{
					_stream.Seek(_endPosition, SeekOrigin.Begin);
				}
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
			_messageSerializer = new MessageSerializer(typeModel);
			_typeTable = typeTable;
		}

		public void SetSerializeWrapKey(int wrapKey, bool pending = false)
		{
			if (!pending)
			{
				_serializeWrapKey = wrapKey;
			}
			else
			{
				_serializeWrapPendingKey = wrapKey;
			}
		}

		public void SetDeserializeWrapKey(int wrapKey)
		{
			_deserializeWrapKey = wrapKey;
		}

		public int EstimateLength(object packet)
		{
			return 0;
		}

		public void Serialize(Stream stream, object packet)
		{
			Packet p = (Packet)packet;
			StreamLengthMarker packetMarker = new StreamLengthMarker(stream, false);
			stream.Seek(8L, SeekOrigin.Current);
			int header = (int)p.Type | ((p.Message != null) ? 0x80 : 0) | ((p.AdditionalMessages != null && p.AdditionalMessages.Length != 0) ? 0x40 : 0);
			stream.WriteByte((byte)header);
			switch (p.Type)
			{
			case PacketType.Request:
				stream.Write7BitEncodedInt(p.RequestId);
				break;
			case PacketType.Reply:
				stream.Write7BitEncodedInt(p.RequestId);
				stream.WriteZigZag7BitEncodedInt(p.ReplyCode);
				break;
			}
			if (p.Message != null)
			{
				stream.Write7BitEncodedInt(_typeTable.GetAlias(p.Message.GetType()));
				StreamLengthMarker messageMarker = new StreamLengthMarker(stream, true);
				_messageSerializer.Serialize(stream, p.Message);
				messageMarker.WriteLength(true);
			}
			if (p.AdditionalMessages != null && p.AdditionalMessages.Length > 0)
			{
				stream.Write7BitEncodedInt(p.AdditionalMessages.Length);
				for (int i = 0; i < p.AdditionalMessages.Length; i++)
				{
					MessageBase additional = p.AdditionalMessages[i];
					stream.Write7BitEncodedInt(_typeTable.GetAlias(additional.GetType()));
					StreamLengthMarker additionalMarker = new StreamLengthMarker(stream, true);
					_messageSerializer.Serialize(stream, additional);
					additionalMarker.WriteLength(true);
				}
			}
			packetMarker.WriteLength(false);
			long endPosition = packetMarker.EndPosition;
			GetBuffers(stream, (int)packetMarker.StartPosition + 8, (int)(endPosition - packetMarker.StartPosition) - 8, out var segment0, out var segment1);
			EncryptContext ctx = default(EncryptContext);
			ctx.Key = _serializeWrapKey;
			Encrypt(segment0.Array, segment0.Offset, segment0.Array, segment0.Offset, segment0.Count, ref ctx);
			Encrypt(segment1.Array, segment1.Offset, segment1.Array, segment1.Offset, segment1.Count, ref ctx);
			if (_serializeWrapKey != 0)
			{
				_serializeWrapKey++;
				if (_serializeWrapKey == 0)
				{
					_serializeWrapKey = 1;
				}
			}
			byte[] hash = BitConverter.GetBytes(ctx.Hash);
			stream.Write(hash, 0, hash.Length);
			stream.Seek(endPosition, SeekOrigin.Begin);
			if (_serializeWrapPendingKey != 0)
			{
				_serializeWrapKey = _serializeWrapPendingKey;
				_serializeWrapPendingKey = 0;
			}
		}

		public int PeekLength(Stream stream)
		{
			if (stream.Length - stream.Position < 4)
			{
				return 0;
			}
			byte[] buffer = new byte[4];
			stream.Read(buffer, 0, 4);
			stream.Seek(-4L, SeekOrigin.Current);
			return BitConverter.ToInt32(buffer, 0) + 4;
		}

		public object Deserialize(Stream stream)
		{
			Packet packet = new Packet();
			int startPosition = (int)stream.Position;
			int length = stream.Read32BitEncodedInt();
			int expectedHash = stream.Read32BitEncodedInt();
			GetBuffers(stream, startPosition + 8, length - 4, out var segment0, out var segment1);
			DecryptContext ctx = default(DecryptContext);
			ctx.Key = _deserializeWrapKey;
			Decrypt(segment0.Array, segment0.Offset, segment0.Array, segment0.Offset, segment0.Count, ref ctx);
			Decrypt(segment1.Array, segment1.Offset, segment1.Array, segment1.Offset, segment1.Count, ref ctx);
			if (ctx.Hash != expectedHash)
			{
				throw new IOException("Hash mismatch");
			}
			if (_deserializeWrapKey != 0)
			{
				_deserializeWrapKey++;
				if (_deserializeWrapKey == 0)
				{
					_deserializeWrapKey = 1;
				}
			}
			int header = stream.ReadByte();
			packet.Type = (PacketType)(header & 3);
			switch (packet.Type)
			{
			case PacketType.Simple:
				break;
			case PacketType.Request:
				packet.RequestId = stream.Read7BitEncodedInt();
				break;
			case PacketType.Reply:
				packet.RequestId = stream.Read7BitEncodedInt();
				packet.ReplyCode = stream.ReadZigZag7BitEncodedInt();
				break;
			default:
				throw new Exception("Illegal packet type: " + packet.Type.ToString());
			}
			if ((header & 0x80) != 0)
			{
				int messageId = stream.Read7BitEncodedInt();
				int messageLength = stream.Read32BitEncodedInt();
				Type messageType = _typeTable.GetType(messageId);
				if (messageType == null)
				{
					throw new Exception("Illegal messageId: " + messageId.ToString());
				}
				packet.Message = (MessageBase)Activator.CreateInstance(messageType);
				_messageSerializer.Deserialize(stream, packet.Message, messageType, messageLength);
			}
			if ((header & 0x40) != 0)
			{
				int count = stream.Read7BitEncodedInt();
				packet.AdditionalMessages = new MessageBase[count];
				for (int i = 0; i < count; i++)
				{
					int messageId = stream.Read7BitEncodedInt();
					int messageLength = stream.Read32BitEncodedInt();
					Type messageType = _typeTable.GetType(messageId);
					if (messageType == null)
					{
						throw new Exception("Illegal messageId: " + messageId.ToString());
					}
					packet.AdditionalMessages[i] = (MessageBase)Activator.CreateInstance(messageType);
					_messageSerializer.Deserialize(stream, packet.AdditionalMessages[i], messageType, messageLength);
				}
			}
			int consumed = (int)stream.Position - startPosition;
			if (length + 4 != consumed)
			{
				// TODO(decomp): the first operand of the message comes from an unnamed helper @0x184395e40 (assumed length + 4); the second is the consumed byte count.
				throw new Exception("Mismatched length: " + (length + 4).ToString() + " " + consumed.ToString());
			}
			return packet;
		}

		public ParseHeaderResult TryParseHeader(Stream stream, out Type type, out int messageLen, out object extraInfo)
		{
			type = null;
			messageLen = 0;
			extraInfo = null;
			try
			{
				long remaining = stream.Length - stream.Position;
				if (remaining < 28)
				{
					extraInfo = remaining;
					return ParseHeaderResult.NotEnoughDataReceived;
				}
				int startPosition = (int)stream.Position;
				stream.Read32BitEncodedInt();
				stream.Read32BitEncodedInt();
				GetBuffers(stream, startPosition + 8, 20, out var segment0, out var segment1);
				DecryptContext ctx = default(DecryptContext);
				ctx.Key = _deserializeWrapKey;
				Decrypt(segment0.Array, segment0.Offset, segment0.Array, segment0.Offset, segment0.Count, ref ctx);
				Decrypt(segment1.Array, segment1.Offset, segment1.Array, segment1.Offset, segment1.Count, ref ctx);
				int header = stream.ReadByte();
				PacketType packetType = (PacketType)(header & 3);
				switch (packetType)
				{
				case PacketType.Simple:
					break;
				case PacketType.Request:
					stream.Read7BitEncodedInt();
					break;
				case PacketType.Reply:
					stream.Read7BitEncodedInt();
					stream.Read7BitEncodedInt();
					break;
				default:
					extraInfo = packetType;
					return ParseHeaderResult.InvalidPacketType;
				}
				if ((header & 0x80) != 0)
				{
					int messageId = stream.Read7BitEncodedInt();
					messageLen = stream.Read32BitEncodedInt();
					type = _typeTable.GetType(messageId);
					if (type == null)
					{
						extraInfo = messageId;
						return ParseHeaderResult.FailedToGetMessageId;
					}
				}
				extraInfo = (header & 0x40) != 0;
				return ParseHeaderResult.Ok;
			}
			catch (Exception e)
			{
				extraInfo = e;
				return ParseHeaderResult.ExceptionOccured;
			}
		}

		protected void GetBuffers(Stream stream, int pos, int length, out ArraySegment<byte> segment0, out ArraySegment<byte> segment1)
		{
			if (stream is MemoryStream memoryStream)
			{
				segment0 = new ArraySegment<byte>(memoryStream.GetBuffer(), pos, length);
				segment1 = default;
				return;
			}
			if (stream is HeadTailWriteStream headTailWriteStream)
			{
				headTailWriteStream.GetBuffers(pos, length, out segment0, out segment1);
				return;
			}
			throw new InvalidOperationException("Unknown stream!");
		}

		private static void Encrypt(byte[] target, int targetOffset, byte[] source, int sourceOffset, int length, ref EncryptContext ctx)
		{
			for (int i = 0; i < length; i++)
			{
				byte b = source[sourceOffset + i];
				ctx.Hash ^= b << (ctx.Index % 4 * 8);
				target[targetOffset + i] = (byte)(b ^ (byte)ctx.Key);
				ctx.Index++;
			}
		}

		private static void Decrypt(byte[] target, int targetOffset, byte[] source, int sourceOffset, int length, ref DecryptContext ctx)
		{
			for (int i = 0; i < length; i++)
			{
				byte b = (byte)(source[sourceOffset + i] ^ (byte)ctx.Key);
				ctx.Hash ^= b << (ctx.Index % 4 * 8);
				target[targetOffset + i] = b;
				ctx.Index++;
			}
		}
	}
}
