using ProtoBuf;

[ProtoContract]
[Request]
public class ReqCheatSpawnWaveExecutor : BaseMessage
{
	[ProtoMember(1)]
	public int WaveExecutorId { get; set; }
}
