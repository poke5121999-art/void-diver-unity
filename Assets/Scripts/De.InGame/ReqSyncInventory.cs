using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSyncInventory : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }
}
