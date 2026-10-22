using ProtoBuf;

[ProtoContract]
[Response]
public class ResLevelUp : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }

	[ProtoMember(3)]
	public int Level { get; set; }

	[ProtoMember(4)]
	public long ConsumeGold { get; set; }

	[ProtoMember(5)]
	public long ConsumeCoin { get; set; }
}
