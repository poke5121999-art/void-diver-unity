using GameCommon.Data;
using GameCommon.Enum;

public class StageEndEvent : IBaseEvent
{
	public int CampaignId { get; private set; }

	public EDifficulty Difficulty { get; private set; }

	public int Players { get; private set; }

	public bool IsEscaped { get; private set; }

	public StageEndEvent(int campaignId, EDifficulty difficulty, int players, bool isEscaped)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
