using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatSpawnMonster : BaseMessage
{
	[ProtoMember(1)]
	public int MonsterId { get; set; }

	[ProtoMember(2)]
	public int MonsterCount { get; set; }
}
