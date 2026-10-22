using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRemoveSafeSlot : BaseMessage
{
	[ProtoMember(1)]
	public int SlotIndex { get; set; }
}
