using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSetCharacterStress : BaseMessage
{
	[ProtoMember(1)]
	public float Stress { get; set; }
}
