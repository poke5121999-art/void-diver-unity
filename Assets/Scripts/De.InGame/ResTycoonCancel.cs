using ProtoBuf;

[ProtoContract]
[Response]
public class ResTycoonCancel : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }
}
