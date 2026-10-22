using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSetCharacterCorruption : BaseMessage
{
	[ProtoMember(1)]
	public int Corruption { get; set; }
}
