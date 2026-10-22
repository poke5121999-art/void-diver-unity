using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateUnitModel : BaseMessage
{
	[ProtoMember(1)]
	public int Tick { get; set; }

	[ProtoMember(2)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(3)]
	public UnitBuff[] ChangeBuffs { get; set; }

	[ProtoMember(4)]
	public UnitSkill[] ChangeSkills { get; set; }

	[ProtoMember(5)]
	public UnitItemCoolTime[] ChangeConsumableTypeCoolTimes { get; set; }

	[ProtoMember(6)]
	public UnitStat[] ChangeStats { get; set; }

	[ProtoMember(7)]
	public UnitCc[] ChangeCrowdControls { get; set; }
}
