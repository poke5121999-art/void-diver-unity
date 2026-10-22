using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSafeSlotToStashSlot : BaseMessage
{
	[ProtoMember(1)]
	public int SafeSlotIndex { get; set; }

	[ProtoMember(2)]
	public int StashSlotIndex { get; set; }

	[ProtoMember(3)]
	public long Count { get; set; }
}
