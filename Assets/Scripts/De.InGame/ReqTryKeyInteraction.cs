using ProtoBuf;

[ProtoContract]
[Request]
public class ReqTryKeyInteraction : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public ulong InteractiveObjectId { get; set; }

	[ProtoMember(3)]
	public int[] KeyItemIds { get; set; }
}
