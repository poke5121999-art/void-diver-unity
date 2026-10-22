using ProtoBuf;

[ProtoContract]
[Request]
public class ReqInventorySlotToLootingSlot : BaseMessage
{
	[ProtoMember(1)]
	public int InventorySlotIndex { get; set; }

	[ProtoMember(2)]
	public ulong LootingNetworkObjectId { get; set; }

	[ProtoMember(3)]
	public int LootingSlotIndex { get; set; }

	[ProtoMember(4)]
	public long Count { get; set; }
}
