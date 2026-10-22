using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
[Request]
public class ReqSpawnWaveExecutor : BaseMessage
{
	[ProtoMember(1)]
	public int WaveExecutorId { get; set; }

	[ProtoMember(2)]
	public ProtoVector3 Position { get; set; }

	[ProtoMember(3)]
	public bool ExecuteImmediately { get; set; }
}
