using GameCommon.Data;
using GameCommon.Enum;

public class CampaignEndEvent : IBaseEvent
{
	public int CampaignId { get; private set; }

	public EDifficulty Difficulty { get; private set; }

	public int Players { get; private set; }

	public ECampaignCloseReason Reason { get; private set; }

	public CampaignEndEvent(int campaignId, EDifficulty difficulty, int players, ECampaignCloseReason reason)
	{
		CampaignId = campaignId;
		Difficulty = difficulty;
		Players = players;
		Reason = reason;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}
