using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRepairGoods : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public EGoodsSlotCategory SlotCategory { get; set; }

	[ProtoMember(3)]
	public int SlotIndex { get; set; }

	[ProtoMember(4)]
	public bool PreventMaxDurabilityLoss { get; set; }
}
