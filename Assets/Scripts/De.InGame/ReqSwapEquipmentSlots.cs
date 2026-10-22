using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSwapEquipmentSlots : BaseMessage
{
	[ProtoMember(1)]
	public EGoodsSlotCategory EquipmentSlotCategory { get; set; }

	[ProtoMember(2)]
	public int SourceSlotIndex { get; set; }

	[ProtoMember(3)]
	public int TargetSlotIndex { get; set; }
}
