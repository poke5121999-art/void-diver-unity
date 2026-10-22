using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSetCharacterLightFuel : BaseMessage
{
	[ProtoMember(1)]
	public int LightFuel { get; set; }
}
