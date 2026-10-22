using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfSkillIndicator : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public ESkillIndicatorType IndicatorType { get; set; }

	[ProtoMember(3)]
	public float CastRange { get; set; }

	[ProtoMember(4)]
	public float EffectRange { get; set; }

	[ProtoMember(5)]
	public float Length { get; set; }

	[ProtoMember(6)]
	public float Angle { get; set; }
}
