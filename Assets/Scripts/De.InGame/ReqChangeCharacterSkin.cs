using ProtoBuf;

[ProtoContract]
[Request]
public class ReqChangeCharacterSkin : BaseMessage
{
	[ProtoMember(1)]
	public int SkinPresetId { get; set; }
}
