using ProtoBuf;

[ProtoContract]
[Request]
public class ReqVoteCampaign : BaseMessage
{
	[ProtoMember(1)]
	public bool IsAgreed { get; set; }
}
