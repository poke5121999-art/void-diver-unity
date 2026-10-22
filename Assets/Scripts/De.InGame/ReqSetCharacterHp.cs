using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSetCharacterHp : BaseMessage
{
	[ProtoMember(1)]
	public float Hp { get; set; }
}
