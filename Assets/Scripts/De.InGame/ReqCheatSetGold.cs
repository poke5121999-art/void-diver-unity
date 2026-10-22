using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatSetGold : BaseMessage
{
	[ProtoMember(1)]
	public long Gold { get; set; }
}
