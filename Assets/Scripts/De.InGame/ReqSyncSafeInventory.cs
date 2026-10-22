using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSyncSafeInventory : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }
}
