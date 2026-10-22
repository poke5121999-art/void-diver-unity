using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfHpDamage : BaseMessage
{
	[ProtoMember(1)]
	public ulong SourceNetworkObjectId { get; set; }

	[ProtoMember(2)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(3)]
	public int Amount { get; set; }

	[ProtoMember(4)]
	public ProtoVector3 HitPosition { get; set; }

	[ProtoMember(5)]
	public bool IsCritical { get; set; }

	[ProtoMember(6)]
	public EElementalDamagedType ElementalDamagedType { get; set; }

	[ProtoMember(7)]
	public bool IsBackAttack { get; set; }

	[ProtoMember(8)]
	public EHpDamageReasonType ReasonType { get; set; }
}
