using GameCommon.Data;
using GameCommon.Enum;

public class StageStartEvent : IBaseEvent
{
	public int CampaignId { get; private set; }

	public EDifficulty Difficulty { get; private set; }

	public int Players { get; private set; }

	public StageStartEvent(int campaignId, EDifficulty difficulty, int players)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
