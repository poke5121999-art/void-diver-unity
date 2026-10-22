using ProtoBuf;

[ProtoContract]
[Request]
public class ReqDropSafeSlot : BaseMessage
{
	[ProtoMember(1)]
	public int SafeSlotIndex { get; set; }

	[ProtoMember(2)]
	public long Count { get; set; }
}
