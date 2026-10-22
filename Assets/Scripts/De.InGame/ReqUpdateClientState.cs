using ProtoBuf;

[ProtoContract]
[Request]
public class ReqUpdateClientState : BaseMessage
{
	[ProtoMember(1)]
	public EClientState State { get; set; }
}
