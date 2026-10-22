using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResInventorySlotToStashBagSlot : BaseMessage
{
	[ProtoMember(1)]
	public bool IsSuccess { get; set; }

	[ProtoMember(2)]
	public int StashBagHolderSlotIndex { get; set; }

	[ProtoMember(3)]
	public int InnerSlotIndex { get; set; }

	[ProtoMember(4)]
	public Goods PulledGoods { get; set; }
}
