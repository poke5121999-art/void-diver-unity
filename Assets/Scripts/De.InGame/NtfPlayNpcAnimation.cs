using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfPlayNpcAnimation : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public string AnimationName { get; set; }

	[ProtoMember(3)]
	public bool IsLoop { get; set; }
}
