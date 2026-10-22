using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSwapWeapon : BaseMessage
{
	[ProtoMember(1)]
	public bool AllowEmpty { get; set; }
}
