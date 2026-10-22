using ProtoBuf;

[ProtoContract]
[Request]
public class ReqLootingSlotToInventorySlot : BaseMessage
{
	[ProtoMember(1)]
	public ulong LootingNetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int LootingSlotIndex { get; set; }

	[ProtoMember(3)]
	public int InventorySlotIndex { get; set; }

	[ProtoMember(4)]
	public long Count { get; set; }
}
