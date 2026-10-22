using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfEquipmentBroken : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int EquipmentId { get; set; }
}
