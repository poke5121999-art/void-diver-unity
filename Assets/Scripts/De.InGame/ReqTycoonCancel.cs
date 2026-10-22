using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqTycoonCancel : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int SalesSlotIndex { get; set; }

	[ProtoMember(3)]
	public Goods Artifact { get; set; }
}
