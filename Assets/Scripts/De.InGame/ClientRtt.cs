using ProtoBuf;

[ProtoContract]
public class ClientRtt
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public ulong Rtt { get; set; }
}
