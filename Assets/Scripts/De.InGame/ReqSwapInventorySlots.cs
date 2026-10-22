using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSwapInventorySlots : BaseMessage
{
	[ProtoMember(1)]
	public int SourceSlotIndex { get; set; }

	[ProtoMember(2)]
	public int TargetSlotIndex { get; set; }
}
