using ProtoBuf;

[ProtoContract]
[Request]
public class ReqInventorySlotToStashSlot : BaseMessage
{
	[ProtoMember(1)]
	public int InventorySlotIndex { get; set; }

	[ProtoMember(2)]
	public int StashSlotIndex { get; set; }

	[ProtoMember(3)]
	public long Count { get; set; }
}
