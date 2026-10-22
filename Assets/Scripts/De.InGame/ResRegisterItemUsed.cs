using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResRegisterItemUsed : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }

	[ProtoMember(3)]
	public int ItemId { get; set; }

	[ProtoMember(4)]
	public EGoodsSlotCategory SlotCategory { get; set; }

	[ProtoMember(5)]
	public int SlotIndex { get; set; }

	[ProtoMember(6)]
	public Goods ConsumeGoods { get; set; }
}
