using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResEquipLootingSlot : BaseMessage
{
	[ProtoMember(1)]
	public Goods EquippedGoods { get; set; }
}
