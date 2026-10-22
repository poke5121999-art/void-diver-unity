using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSyncUnitModel : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }
}
