using Unity.Netcode;

public struct Packet : INetworkSerializable
{
	public BaseMessage Message;

	private byte[] messageBytes;

	private const int NamedMessageOverhead = 32;

	public bool HostOnly { get; private set; }

	public string DropReason { get; private set; }

	private int DataSize => 0;

	public int PacketSize => 0;

	public NetworkDelivery GetDelivery(int maxNonFragmentedSize)
	{
		return NetworkDelivery.Unreliable;
	}

	public Packet(BaseMessage message, bool hostOnly = false)
	{
		Message = null;
		HostOnly = false;
		DropReason = null;
		messageBytes = null;
	}

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
	}

	private static void WriteVarint(FastBufferWriter writer, ulong value)
	{
	}

	private static ulong ReadVarint(FastBufferReader reader)
	{
		return 0uL;
	}
}
