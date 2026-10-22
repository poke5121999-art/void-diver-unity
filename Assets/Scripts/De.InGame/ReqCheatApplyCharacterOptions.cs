using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatApplyCharacterOptions : BaseMessage
{
	[ProtoMember(1)]
	public bool Invisible { get; set; }

	[ProtoMember(2)]
	public bool Invincibility { get; set; }

	[ProtoMember(3)]
	public bool SpeedUp { get; set; }
}
