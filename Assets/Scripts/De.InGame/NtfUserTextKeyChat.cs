using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUserTextKeyChat : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public string PlayerName { get; set; }

	[ProtoMember(3)]
	public string TextKey { get; set; }
}
