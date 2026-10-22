using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateAnimation : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public string AnimationName { get; set; }

	[ProtoMember(3)]
	public string PrevAnimationName { get; set; }
}
