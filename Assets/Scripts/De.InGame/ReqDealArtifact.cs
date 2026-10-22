using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqDealArtifact : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public EGoodsSlotCategory SlotCategory { get; set; }

	[ProtoMember(3)]
	public int SlotIndex { get; set; }

	[ProtoMember(4)]
	public Goods DealGoods { get; set; }

	[ProtoMember(5)]
	public long GainCoin { get; set; }

	[ProtoMember(6)]
	public long GainExp { get; set; }
}
