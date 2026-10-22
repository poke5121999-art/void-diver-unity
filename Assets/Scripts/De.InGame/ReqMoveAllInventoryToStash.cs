using ProtoBuf;

[ProtoContract]
[Request]
public class ReqMoveAllInventoryToStash : BaseMessage
{
	[ProtoMember(1)]
	public int[] InventorySlotIndices { get; set; }
}
