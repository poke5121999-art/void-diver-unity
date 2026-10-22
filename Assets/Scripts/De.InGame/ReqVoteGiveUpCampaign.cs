using ProtoBuf;

[ProtoContract]
[Request]
public class ReqVoteGiveUpCampaign : BaseMessage
{
	[ProtoMember(1)]
	public bool IsAgreed { get; set; }
}
