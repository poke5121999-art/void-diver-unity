using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfOnHitInvincibility : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }
}
