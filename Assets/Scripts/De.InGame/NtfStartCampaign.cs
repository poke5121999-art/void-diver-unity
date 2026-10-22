using GameCommon.Enum;
using ProtoBuf;

[ProtoContract]
[Notify]
public class NtfStartCampaign : BaseMessage
{
	[ProtoMember(1)]
	public int CampaignId { get; set; }

	[ProtoMember(2)]
	public EDifficulty Difficulty { get; set; }

	[ProtoMember(3)]
	public EAnomalyType AnomalyType { get; set; }
}
