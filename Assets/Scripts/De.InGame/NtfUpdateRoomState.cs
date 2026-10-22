using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateRoomState : BaseMessage
{
	[ProtoMember(1)]
	public ERoomState State { get; set; }
}
