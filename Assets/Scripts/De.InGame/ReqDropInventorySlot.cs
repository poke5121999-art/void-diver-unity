using ProtoBuf;

[ProtoContract]
[Request]
public class ReqDropInventorySlot : BaseMessage
{
	[ProtoMember(1)]
	public int InventorySlotIndex { get; set; }

	[ProtoMember(2)]
	public long Count { get; set; }
}
