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
		// TODO(decomp): generic body has no address (0x0); reconstructed from WriteVarint/ReadVarint and fields @0x0
		if (serializer.IsReader)
		{
			FastBufferReader fastBufferReader = serializer.GetFastBufferReader();
			fastBufferReader.ReadValueSafe(out bool value);
			HostOnly = value;
			int num = (int)ReadVarint(fastBufferReader);
			messageBytes = new byte[num];
			for (int i = 0; i < num; i++)
			{
				fastBufferReader.ReadByteSafe(out messageBytes[i]);
			}
			Message = SerializeHelper.Deserialize<BaseMessage>(messageBytes);
		}
		else
		{
			FastBufferWriter fastBufferWriter = serializer.GetFastBufferWriter();
			fastBufferWriter.WriteValueSafe(HostOnly);
			WriteVarint(fastBufferWriter, (ulong)DataSize);
			for (int j = 0; j < DataSize; j++)
			{
				fastBufferWriter.WriteByteSafe(messageBytes[j]);
			}
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
