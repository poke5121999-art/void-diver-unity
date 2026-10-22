using ProtoBuf;

[ProtoContract]
[Request]
public class ReqInputMoveAim : BaseMessage
{
	[ProtoMember(1)]
	public uint PackedForward { get; set; }

	[ProtoMember(2)]
	public uint PackedAim { get; set; }
}
