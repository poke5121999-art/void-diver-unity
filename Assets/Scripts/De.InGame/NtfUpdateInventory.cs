using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateInventory : BaseMessage
{
	[ProtoMember(1)]
	public int Tick { get; set; }

	[ProtoMember(2)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(3)]
	public ulong IndividualClientId { get; set; }

	[ProtoMember(4)]
	public GoodsSlot[] ChangeSlots { get; set; }
}
