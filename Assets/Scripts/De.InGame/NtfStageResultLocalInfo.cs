using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfStageResultLocalInfo : BaseMessage
{
	[ProtoMember(1)]
	public StageResultLocalInfo LocalInfo { get; set; }
}
