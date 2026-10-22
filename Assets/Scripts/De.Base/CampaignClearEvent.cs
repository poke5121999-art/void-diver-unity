using GameCommon.Data;

public class CampaignClearEvent : IBaseEvent
{
	public int CampaignId { get; private set; }

	public bool IsFirstClear { get; private set; }

	public CampaignClearEvent(int campaignId, bool isFirstClear = false)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
