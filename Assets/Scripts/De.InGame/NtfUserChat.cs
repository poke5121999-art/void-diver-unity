using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUserChat : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public string Text { get; set; }

	[ProtoMember(3)]
	public long HostUtcSec { get; set; }
}
