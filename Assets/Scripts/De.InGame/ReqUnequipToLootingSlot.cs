using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqUnequipToLootingSlot : BaseMessage
{
	[ProtoMember(1)]
	public EGoodsSlotCategory EquipmentSlotCategory { get; set; }

	[ProtoMember(2)]
	public int EquipmentSlotIndex { get; set; }

	[ProtoMember(3)]
	public ulong LootingNetworkObjectId { get; set; }

	[ProtoMember(4)]
	public int LootingSlotIndex { get; set; }
}
