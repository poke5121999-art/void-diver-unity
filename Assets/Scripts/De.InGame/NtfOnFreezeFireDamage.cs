using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfOnFreezeFireDamage : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }
}
