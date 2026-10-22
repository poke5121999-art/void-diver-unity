using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatAddParadox : BaseMessage
{
	[ProtoMember(1)]
	public int ParadoxId { get; set; }
}
