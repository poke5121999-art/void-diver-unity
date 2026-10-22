using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfItemUsed : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public int ItemId { get; set; }
}
