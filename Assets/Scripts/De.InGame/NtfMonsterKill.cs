using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfMonsterKill : BaseMessage
{
	[ProtoMember(1)]
	public ulong SourceClientId { get; set; }

	[ProtoMember(2)]
	public ulong MonsterNetworkObjectId { get; set; }

	[ProtoMember(3)]
	public int MonsterId { get; set; }
}
