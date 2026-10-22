using GameCommon.Data;
using GameCommon.Enum;

public class CampaignStartEvent : IBaseEvent
{
	public int CampaignId { get; private set; }

	public EDifficulty Difficulty { get; private set; }

	public int Players { get; private set; }

	public CampaignStartEvent(int campaignId, EDifficulty difficulty, int players)
	{
		CampaignId = campaignId;
		Difficulty = difficulty;
		Players = players;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}
