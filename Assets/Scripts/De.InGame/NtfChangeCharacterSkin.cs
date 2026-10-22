using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfChangeCharacterSkin : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public bool IsSuper { get; set; }
}
