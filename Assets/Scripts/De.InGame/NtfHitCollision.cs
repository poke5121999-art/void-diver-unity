using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfHitCollision : BaseMessage
{
	[ProtoMember(1)]
	public ulong SourceNetworkObjectId { get; set; }

	[ProtoMember(2)]
	public ulong TargetNetworkObjectId { get; set; }
}
