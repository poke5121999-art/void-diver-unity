using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfStageResultPlayerInfo : BaseMessage
{
	[ProtoMember(1)]
	public StageResultPlayerInfo PlayerInfo { get; set; }
}
