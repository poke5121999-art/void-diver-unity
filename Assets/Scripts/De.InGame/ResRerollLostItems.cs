using ProtoBuf;

[ProtoContract]
[Response]
public class ResRerollLostItems : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }

	[ProtoMember(3)]
	public long ConsumeGold { get; set; }
}
