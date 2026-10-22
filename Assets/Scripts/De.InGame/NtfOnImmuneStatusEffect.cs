using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfOnImmuneStatusEffect : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public EStatusEffectTag StatusEffectTag { get; set; }

	[ProtoMember(3)]
	public bool IsFullyImmune { get; set; }
}
