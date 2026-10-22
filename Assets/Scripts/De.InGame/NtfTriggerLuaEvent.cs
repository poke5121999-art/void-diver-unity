using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfTriggerLuaEvent : BaseMessage
{
	[ProtoMember(1)]
	public int Value { get; set; }
}
