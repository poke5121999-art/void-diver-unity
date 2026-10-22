using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResCraftingGoods : BaseMessage
{
	[ProtoMember(1)]
	public override int Sequence { get; set; }

	[ProtoMember(2)]
	public bool IsSuccess { get; set; }

	[ProtoMember(3)]
	public int CraftingId { get; set; }

	[ProtoMember(4)]
	public Goods[] RemainMaterials { get; set; }

	[ProtoMember(5)]
	public long ConsumeGold { get; set; }

	[ProtoMember(6)]
	public Goods[] RemainGoods { get; set; }
}
