using GameCommon.Data;
using GameCommon.Enum;

public class PlayCampaignEndEvent : IBaseEvent
{
	public int CampaignId { get; }

	public int Round { get; }

	public int CharacterId { get; }

	public ECampaignCloseReason Reason { get; }

	public int PlayerCount { get; }

	public int? AnomalyId { get; }

	public PlayCampaignEndEvent(int campaignId, int round, int characterId, ECampaignCloseReason reason, int playerCount = 1, int? anomalyId = null)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
