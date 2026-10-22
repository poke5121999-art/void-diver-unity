using ProtoBuf;

[ProtoContract]
[Request]
public class ReqClientIntroFinished : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }
}
