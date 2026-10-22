using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfVoteCampaign : BaseMessage
{
	[ProtoMember(1)]
	public ulong ClientId { get; set; }

	[ProtoMember(2)]
	public int CampaignId { get; set; }

	[ProtoMember(3)]
	public EDifficulty Difficulty { get; set; }

	[ProtoMember(4)]
	public bool IsAgreed { get; set; }

	[ProtoMember(5)]
	public EAnomalyType AnomalyType { get; set; }

	[ProtoMember(6)]
	public bool IsVoteStart { get; set; }
}
