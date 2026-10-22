using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResTycoonFinish : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }

	[ProtoMember(3)]
	public ETycoonFailType FailType { get; set; }

	[ProtoMember(4)]
	public long GainCoin { get; set; }

	[ProtoMember(5)]
	public Goods ReturnArtifact { get; set; }
}
