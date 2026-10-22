using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRest : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }
}
