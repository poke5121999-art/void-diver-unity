using ProtoBuf;

[ProtoContract]
[Request]
public class ReqStageResultPlayerLocalInfo : BaseMessage
{
	[ProtoMember(1)]
	public StageResultLocalInfo LocalInfo { get; set; }
}
