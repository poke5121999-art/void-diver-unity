using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSwapLootingSlots : BaseMessage
{
	[ProtoMember(1)]
	public ulong LootingNetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int SourceSlotIndex { get; set; }

	[ProtoMember(3)]
	public int TargetSlotIndex { get; set; }
}
