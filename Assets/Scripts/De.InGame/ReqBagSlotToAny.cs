using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqBagSlotToAny : BaseMessage
{
	[ProtoMember(1)]
	public EGoodsSlotCategory BagHolderCategory { get; set; }

	[ProtoMember(2)]
	public int BagHolderSlotIndex { get; set; }

	[ProtoMember(3)]
	public ulong BagHolderNetworkObjectId { get; set; }

	[ProtoMember(4)]
	public int InnerSlotIndex { get; set; }

	[ProtoMember(5)]
	public EGoodsSlotCategory TargetCategory { get; set; }

	[ProtoMember(6)]
	public int TargetSlotIndex { get; set; }

	[ProtoMember(7)]
	public ulong TargetNetworkObjectId { get; set; }

	[ProtoMember(8)]
	public long Count { get; set; }
}
