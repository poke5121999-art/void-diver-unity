using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfRemoveFx : BaseMessage
{
	[ProtoMember(1)]
	public long UniqueId { get; set; }
}
