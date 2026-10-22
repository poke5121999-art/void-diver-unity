using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqAnyToBagSlot : BaseMessage
{
	[ProtoMember(1)]
	public EGoodsSlotCategory SourceCategory { get; set; }

	[ProtoMember(2)]
	public int SourceSlotIndex { get; set; }

	[ProtoMember(3)]
	public ulong SourceNetworkObjectId { get; set; }

	[ProtoMember(4)]
	public Goods SourceGoods { get; set; }

	[ProtoMember(5)]
	public EGoodsSlotCategory BagHolderCategory { get; set; }

	[ProtoMember(6)]
	public int BagHolderSlotIndex { get; set; }

	[ProtoMember(7)]
	public ulong BagHolderNetworkObjectId { get; set; }

	[ProtoMember(8)]
	public int InnerSlotIndex { get; set; }

	[ProtoMember(9)]
	public long Count { get; set; }
}
