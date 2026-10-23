using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
public class MonsterRecord
{
	[ProtoMember(1)]
	public int MonsterGroupId { get; set; }

	[ProtoMember(2)]
	public EMonsterRecordFlags Flags { get; set; }

	[ProtoMember(3)]
	public byte KillCount { get; set; }

	public MonsterRecord Clone()
	{
		return new MonsterRecord
		{
			MonsterGroupId = MonsterGroupId,
			Flags = Flags,
			KillCount = KillCount
		};
	}
}
