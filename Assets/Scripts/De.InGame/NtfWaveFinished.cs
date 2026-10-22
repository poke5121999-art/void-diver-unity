using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfWaveFinished : BaseMessage
{
	[ProtoMember(1)]
	public ulong NetworkObjectId { get; set; }

	[ProtoMember(2)]
	public int WaveId { get; set; }

	[ProtoMember(3)]
	public bool IsClear { get; set; }
}
