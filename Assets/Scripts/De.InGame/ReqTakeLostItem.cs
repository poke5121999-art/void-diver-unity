using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqTakeLostItem : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int SlotIndex { get; set; }

	[ProtoMember(3)]
	public long ConsumeGold { get; set; }

	[ProtoMember(4)]
	public Goods Goods { get; set; }

	[ProtoMember(5)]
	public long ConsumeCoin { get; set; }
}
