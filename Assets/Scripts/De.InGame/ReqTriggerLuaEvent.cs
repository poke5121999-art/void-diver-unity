using ProtoBuf;

[ProtoContract]
[Request]
public class ReqTriggerLuaEvent : BaseMessage
{
	[ProtoMember(1)]
	public int Value { get; set; }
}
