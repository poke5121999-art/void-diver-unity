using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfPlayerLeft : BaseMessage
{
	[ProtoMember(1)]
	public ulong ExitedClientId { get; set; }
}
