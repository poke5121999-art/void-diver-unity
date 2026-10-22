using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfStressDamage : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public float Amount { get; set; }

	[ProtoMember(3)]
	public ProtoVector3 HitPosition { get; set; }

	[ProtoMember(4)]
	public EStressReasonType StressReasonType { get; set; }
}
