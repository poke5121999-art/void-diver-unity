using GameCommon.Data;
using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfDisplayFx : BaseMessage
{
	[ProtoMember(1)]
	public long UniqueId { get; set; }

	[ProtoMember(2)]
	public string Vfx { get; set; }

	[ProtoMember(3)]
	public float VfxDuration { get; set; }

	[ProtoMember(4)]
	public float VfxScale { get; set; } = 1f;

	[ProtoMember(5)]
	public string Sfx { get; set; }

	[ProtoMember(6)]
	public ProtoVector3 Position { get; set; }

	[ProtoMember(7)]
	public ProtoVector3 Direction { get; set; }

	[ProtoMember(8)]
	public AnimationSpeed[] AnimSpeeds { get; set; }

	[ProtoMember(9)]
	public bool IsLoopVfx { get; set; }

	[ProtoMember(10)]
	public float LoopDuration { get; set; }

	[ProtoMember(11)]
	public ulong SkillOwnerNetworkObjectId { get; set; }

	[ProtoMember(12)]
	public ulong TargetNetworkObjectId { get; set; }

	[ProtoMember(13)]
	public EUnitBoneType BoneType { get; set; }

	[ProtoMember(14)]
	public ProtoVector3 TargetPositionOffset { get; set; }

	[ProtoMember(15)]
	public bool InheritMoveDir { get; set; }

	[ProtoMember(16)]
	public bool DestroyOnUnitFollowerTargetDeath { get; set; }

	[ProtoMember(17)]
	public bool UpdateByAimDir { get; set; }
}
