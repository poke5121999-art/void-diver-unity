using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfLightFuelRecovery : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int Amount { get; set; }
}
