using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResUnequipToStashSlot : BaseMessage
{
	[ProtoMember(1)]
	public int StashSlotIndex { get; set; }

	[ProtoMember(2)]
	public Goods UnequippedGoods { get; set; }
}
