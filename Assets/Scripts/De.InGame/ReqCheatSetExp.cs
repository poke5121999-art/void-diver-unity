using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatSetExp : BaseMessage
{
	[ProtoMember(1)]
	public long Exp { get; set; }
}
