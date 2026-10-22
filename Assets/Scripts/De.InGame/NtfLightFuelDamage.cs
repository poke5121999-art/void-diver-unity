using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfLightFuelDamage : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int Amount { get; set; }

	[ProtoMember(3)]
	public ProtoVector3 HitPosition { get; set; }
}
