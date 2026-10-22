using ProtoBuf;

[ProtoContract]
[Request]
public class ReqRepairAllGoods : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool PreventMaxDurabilityLoss { get; set; }
}
