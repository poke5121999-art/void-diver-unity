using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCraftingGoods : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public int CraftingId { get; set; }

	[ProtoMember(3)]
	public long Count { get; set; }
}
