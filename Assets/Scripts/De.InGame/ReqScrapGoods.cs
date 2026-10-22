using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqScrapGoods : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int ScrapId { get; set; }

	[ProtoMember(3)]
	public int Count { get; set; }

	[ProtoMember(4)]
	public EGoodsSlotCategory SlotCategory { get; set; }

	[ProtoMember(5)]
	public int SlotIndex { get; set; }
}
