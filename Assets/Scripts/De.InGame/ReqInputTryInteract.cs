using ProtoBuf;

[ProtoContract]
[Request]
public class ReqInputTryInteract : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }
}
