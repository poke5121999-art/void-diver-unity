using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfMonsterTargetChanged : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public ulong TargetClientId { get; set; }

	[ProtoMember(3)]
	public bool IsBoss { get; set; }
}
