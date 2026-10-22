using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfStressRecovery : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public float Amount { get; set; }
}
