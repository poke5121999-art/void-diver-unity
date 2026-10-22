using ProtoBuf;

[ProtoContract]
[Request]
public class ReqUserChat : BaseMessage
{
	[ProtoMember(1)]
	public string Text { get; set; }
}
