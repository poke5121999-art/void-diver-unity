using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSafeSlotToLootingSlot : BaseMessage
{
	[ProtoMember(1)]
	public int SafeSlotIndex { get; set; }

	[ProtoMember(2)]
	public ulong LootingNetworkObjectId { get; set; }

	[ProtoMember(3)]
	public int LootingSlotIndex { get; set; }

	[ProtoMember(4)]
	public long Count { get; set; }
}
