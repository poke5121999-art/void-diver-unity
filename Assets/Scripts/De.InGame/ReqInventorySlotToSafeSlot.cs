using ProtoBuf;

[ProtoContract]
[Request]
public class ReqInventorySlotToSafeSlot : BaseMessage
{
	[ProtoMember(1)]
	public int InventorySlotIndex { get; set; }

	[ProtoMember(2)]
	public int SafeSlotIndex { get; set; }
}
