using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqEquipStashSlot : BaseMessage
{
	[ProtoMember(1)]
	public EGoodsSlotCategory EquipmentSlotCategory { get; set; }

	[ProtoMember(2)]
	public int EquipmentSlotIndex { get; set; }

	[ProtoMember(3)]
	public int StashSlotIndex { get; set; }

	[ProtoMember(4)]
	public Goods EquipmentGoods { get; set; }
}
