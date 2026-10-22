using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqEquipSafeSlot : BaseMessage
{
	[ProtoMember(1)]
	public EGoodsSlotCategory EquipmentSlotCategory { get; set; }

	[ProtoMember(2)]
	public int EquipmentSlotIndex { get; set; }

	[ProtoMember(3)]
	public int SafeSlotIndex { get; set; }
}
