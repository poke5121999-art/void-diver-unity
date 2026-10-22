using ProtoBuf;

[ProtoContract]
public class StageResultLocalInfo
{
	[ProtoMember(1)]
	public ulong ClientId;

	[ProtoMember(2)]
	public long BaseExpFromRewardBox;

	[ProtoMember(3)]
	public long BaseExpFromMonsterKill;

	[ProtoMember(4)]
	public long BaseExpFromEndStage;

	[ProtoMember(5)]
	public long DifficultyBonusExp;

	[ProtoMember(6)]
	public long TalentBonusExp;
}
