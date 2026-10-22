using ProtoBuf;

[ProtoContract]
[Response]
public class ResSyncSafeInventory : BaseMessage
{
	[ProtoMember(1)]
	public int Tick { get; set; }

	[ProtoMember(2)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(3)]
	public GoodsSlot[] SafeSlots { get; set; }
}
