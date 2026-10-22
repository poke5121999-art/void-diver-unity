using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqTycoonRegisterArtifact : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int SalesSlotIndex { get; set; }

	[ProtoMember(3)]
	public EGoodsSlotCategory ArtifactSlotCategory { get; set; }

	[ProtoMember(4)]
	public int ArtifactSlotIndex { get; set; }

	[ProtoMember(5)]
	public Goods Artifact { get; set; }
}
