using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfSelfReviveText : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public int EquipmentId { get; set; }
}
