using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRegisterItemUsed : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int ItemId { get; set; }

	[ProtoMember(3)]
	public EGoodsSlotCategory SlotCategory { get; set; }

	[ProtoMember(4)]
	public int SlotIndex { get; set; }

	[ProtoMember(5)]
	public EGoodsType ExpectedType { get; set; }
}
