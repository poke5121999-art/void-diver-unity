using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfAssignedZones : BaseMessage
{
	[ProtoMember(1)]
	public ZoneData[] ZoneDatas { get; set; }
}
