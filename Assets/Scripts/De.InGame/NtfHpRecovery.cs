using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfHpRecovery : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int Amount { get; set; }

	[ProtoMember(3)]
	public EHpRecoveryReasonType ReasonType { get; set; }
}
