using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfEmoji : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int EmojiId { get; set; }
}
