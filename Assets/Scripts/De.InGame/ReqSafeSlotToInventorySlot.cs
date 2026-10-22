using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSafeSlotToInventorySlot : BaseMessage
{
	[ProtoMember(1)]
	public int SafeSlotIndex { get; set; }

	[ProtoMember(2)]
	public int InventorySlotIndex { get; set; }
}
