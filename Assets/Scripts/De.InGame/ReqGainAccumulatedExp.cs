using ProtoBuf;

[ProtoContract]
[Request]
public class ReqGainAccumulatedExp : BaseMessage
{
	[ProtoMember(1)]
	public long AccumulatedExp { get; set; }
}
