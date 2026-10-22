using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfVoteGiveUpCampaign : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public bool IsAgreed { get; set; }
}
