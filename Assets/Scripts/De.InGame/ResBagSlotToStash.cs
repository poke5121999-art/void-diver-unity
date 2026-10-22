using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Response]
public class ResBagSlotToStash : BaseMessage
{
	[ProtoMember(1)]
	public bool IsSuccess { get; set; }

	[ProtoMember(2)]
	public int StashSlotIndex { get; set; }

	[ProtoMember(3)]
	public Goods PulledGoods { get; set; }
}
