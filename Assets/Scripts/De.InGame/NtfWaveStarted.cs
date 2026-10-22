using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfWaveStarted : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int WaveId { get; set; }

	[ProtoMember(3)]
	public ProtoVector3 StartPosition { get; set; }

	[ProtoMember(4)]
	public float StartTime { get; set; }
}
