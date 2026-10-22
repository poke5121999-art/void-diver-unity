using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRerollLostItems : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public long ConsumeGold { get; set; }
}
