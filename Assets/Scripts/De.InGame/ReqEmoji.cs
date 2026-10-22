using ProtoBuf;

[ProtoContract]
[Request]
public class ReqEmoji : BaseMessage
{
	[ProtoMember(1)]
	public int EmojiId { get; set; }
}
