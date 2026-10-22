using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqStashSlotToSafeSlot : BaseMessage
{
	[ProtoMember(1)]
	public int SafeSlotIndex { get; set; }

	[ProtoMember(2)]
	public Goods Goods { get; set; }
}
