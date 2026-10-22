using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfPlayAnimation : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public string AnimationName { get; set; }

	[ProtoMember(3)]
	public bool IsLoop { get; set; }

	[ProtoMember(4)]
	public float AnimStartOffsetTime { get; set; }

	[ProtoMember(5)]
	public AnimationSpeed[] AnimationSpeeds { get; set; }

	[ProtoMember(6)]
	public string MultiTrackAnimationName { get; set; }
}
