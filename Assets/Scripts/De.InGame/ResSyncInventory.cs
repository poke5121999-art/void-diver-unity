using ProtoBuf;

[ProtoContract]
[Response]
public class ResSyncInventory : BaseMessage
{
	[ProtoMember(1)]
	public int Tick { get; set; }

	[ProtoMember(2)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(3)]
	public ulong IndividualClientId { get; set; }

	[ProtoMember(4)]
	public GoodsSlot[] Slots { get; set; }

	[ProtoMember(5)]
	public int SlotCount { get; set; }
}
