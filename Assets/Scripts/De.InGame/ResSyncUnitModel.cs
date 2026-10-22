using ProtoBuf;

[ProtoContract]
[Response]
public class ResSyncUnitModel : BaseMessage
{
	[ProtoMember(1)]
	public int Tick { get; set; }

	[ProtoMember(2)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(3)]
	public UnitBuff[] Buffs { get; set; }

	[ProtoMember(4)]
	public UnitSkill[] Skills { get; set; }

	[ProtoMember(5)]
	public UnitItemCoolTime[] ItemTypeCoolTimes { get; set; }

	[ProtoMember(6)]
	public UnitStat[] Stats { get; set; }

	[ProtoMember(7)]
	public UnitCc[] Ccs { get; set; }
}
