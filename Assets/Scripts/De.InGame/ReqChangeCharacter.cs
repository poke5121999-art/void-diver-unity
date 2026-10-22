using ProtoBuf;

[ProtoContract]
[Request]
public class ReqChangeCharacter : BaseMessage
{
	[ProtoMember(1)]
	public Character Character { get; set; }
}
