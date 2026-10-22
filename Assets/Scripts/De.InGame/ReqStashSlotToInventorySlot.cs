using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqStashSlotToInventorySlot : BaseMessage
{
	[ProtoMember(1)]
	public int InventorySlotIndex { get; set; }

	[ProtoMember(2)]
	public Goods Goods { get; set; }
}
