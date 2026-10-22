using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfAllPlayerReadyToEnterStage : BaseMessage
{
	[ProtoMember(1)]
	public float EnterTime { get; set; }
}
