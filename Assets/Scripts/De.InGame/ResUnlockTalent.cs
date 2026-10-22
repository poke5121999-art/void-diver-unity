using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResUnlockTalent : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }

	[ProtoMember(3)]
	public int TalentId { get; set; }

	[ProtoMember(4)]
	public Goods[] RemainMaterials { get; set; }

	[ProtoMember(5)]
	public long ConsumeGold { get; set; }
}
