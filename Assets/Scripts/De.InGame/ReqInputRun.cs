using ProtoBuf;

[ProtoContract]
[Request]
public class ReqInputRun : BaseMessage
{
	[ProtoMember(1)]
	public bool Run { get; set; }
}
