using ProtoBuf;

[ProtoContract]
[Request]
public class ReqParadoxPurify : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int ParadoxId { get; set; }
}
