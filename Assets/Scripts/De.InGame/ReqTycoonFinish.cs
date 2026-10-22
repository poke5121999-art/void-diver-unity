using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqTycoonFinish : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int SalesSlotIndex { get; set; }

	[ProtoMember(3)]
	public Goods Artifact { get; set; }

	[ProtoMember(4)]
	public bool IsSuccess { get; set; }

	[ProtoMember(5)]
	public int PriceBonusPercent { get; set; }

	[ProtoMember(6)]
	public long GainExp { get; set; }
}
