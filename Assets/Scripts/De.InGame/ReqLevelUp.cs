using ProtoBuf;

[ProtoContract]
[Request]
public class ReqLevelUp : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }
}
