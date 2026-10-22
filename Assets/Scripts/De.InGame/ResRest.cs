using ProtoBuf;

[ProtoContract]
[Response]
public class ResRest : BaseMessage
{
	[ProtoMember(3)]
	public int RemovedParadoxId;

	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }

	[ProtoMember(4)]
	public long ConsumeGold { get; set; }
}
