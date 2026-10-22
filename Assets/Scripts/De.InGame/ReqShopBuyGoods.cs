using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqShopBuyGoods : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int ProductId { get; set; }

	[ProtoMember(3)]
	public int BuyCount { get; set; }

	[ProtoMember(4)]
	public EGoodsSlotCategory DestinationSlotCategory { get; set; }
}
