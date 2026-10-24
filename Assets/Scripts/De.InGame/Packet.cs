using System;
using Unity.Netcode;

public struct Packet : INetworkSerializable
{
	public BaseMessage Message;

	private byte[] messageBytes;

	private const int NamedMessageOverhead = 32;

	public bool HostOnly { get; private set; }

	public string DropReason { get; private set; }

	private int DataSize
	{
		get
		{
			if (messageBytes == null)
			{
				return 0;
			}
			return messageBytes.Length;
		}
	}

	public int PacketSize
	{
		get
		{
			if (messageBytes == null)
			{
				return 5;
			}
			return messageBytes.Length + 5;
		}
	}

	public NetworkDelivery GetDelivery(int maxNonFragmentedSize)
	{
		if (maxNonFragmentedSize - 32 < PacketSize)
		{
			return NetworkDelivery.ReliableFragmentedSequenced;
		}
		return NetworkDelivery.ReliableSequenced;
	}

	public Packet(BaseMessage message, bool hostOnly = false)
	{
		Message = message;
		HostOnly = hostOnly;
		messageBytes = SerializeHelper.Serialize(message);
		DropReason = null;
	}

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		// Shared body @0x180d2d600 (instantiations @0x180d2d080 reader / @0x180d2d340 writer; machine code checked): one varint holds (DataSize << 1) | HostOnly, then the raw bytes.
		if (serializer.IsReader)
		{
			FastBufferReader reader = serializer.GetFastBufferReader();
			ulong num = ReadVarint(reader);
			HostOnly = (num & 1) != 0;
			int num2 = (int)(num >> 1);
			int num3 = reader.Length - reader.Position;
			if (num2 < 0 || num2 > num3)
			{
				DropReason = string.Format("declared dataSize {0} but only {1} bytes remain (hostOnly={2})", num2, num3, HostOnly);
				messageBytes = Array.Empty<byte>();
				Message = null;
			}
			else
			{
				messageBytes = new byte[num2];
				reader.ReadBytes(ref messageBytes, num2);
				Message = SerializeHelper.Deserialize<BaseMessage>(messageBytes);
			}
		}
		else
		{
			FastBufferWriter writer = serializer.GetFastBufferWriter();
			int dataSize = DataSize;
			WriteVarint(writer, ((ulong)(uint)dataSize << 1) | (HostOnly ? 1uL : 0uL));
			writer.WriteBytesSafe(messageBytes, dataSize);
		}
	}

	private static void WriteVarint(FastBufferWriter writer, ulong value)
	{
		while (value >= 128)
		{
			writer.WriteByteSafe((byte)(value | 0x80));
			value >>= 7;
		}
		writer.WriteByteSafe((byte)value);
	}

	private static ulong ReadVarint(FastBufferReader reader)
	{
		ulong num = 0uL;
		int num2 = 0;
		byte value;
		do
		{
			reader.ReadByteSafe(out value);
			num |= (ulong)(value & 0x7F) << num2;
			num2 += 7;
		}
		while ((value & 0x80) != 0 && num2 < 64);
		return num;
	}
}
