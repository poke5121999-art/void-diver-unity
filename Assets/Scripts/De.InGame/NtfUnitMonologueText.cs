using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUnitMonologueText : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public string TextKey { get; set; }
}
