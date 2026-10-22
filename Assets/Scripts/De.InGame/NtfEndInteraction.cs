using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfEndInteraction : BaseMessage
{
	[ProtoMember(1)]
	public ulong SourceClientId { get; set; }

	[ProtoMember(2)]
	public ulong TargetNetworkObjectId { get; set; }
}
