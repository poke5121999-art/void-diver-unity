using ProtoBuf;

[ProtoContract]
[Request]
public class ReqInputCancelInteract : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }
}
