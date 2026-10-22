using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRemoveInventorySlot : BaseMessage
{
	[ProtoMember(1)]
	public int SlotIndex { get; set; }
}
