using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatApplySoul : BaseMessage
{
	[ProtoMember(1)]
	public int Soul { get; set; }
}
