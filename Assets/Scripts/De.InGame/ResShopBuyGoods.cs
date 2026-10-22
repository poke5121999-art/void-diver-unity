using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResShopBuyGoods : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }

	[ProtoMember(3)]
	public int ProductId { get; set; }

	[ProtoMember(4)]
	public int BuyCount { get; set; }

	[ProtoMember(5)]
	public Goods RemainMaterials { get; set; }

	[ProtoMember(6)]
	public long ConsumeGold { get; set; }

	[ProtoMember(7)]
	public EGoodsSlotCategory DestinationSlotCategory { get; set; }

	[ProtoMember(8)]
	public Goods StashGainGoods { get; set; }
}
