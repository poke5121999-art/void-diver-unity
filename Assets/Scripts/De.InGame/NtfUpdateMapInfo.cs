using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfUpdateMapInfo : BaseMessage
{
	[ProtoMember(1)]
	public int[] MapSize { get; set; }

	[ProtoMember(2)]
	public int[] SectorIds { get; set; }
}
