using GameCommon.Enum;
using System;
using GameCommon.Data;

public class CampaignClearEvent : IBaseEvent
{
	public int CampaignId { get; private set; }

	public bool IsFirstClear { get; private set; }

	public CampaignClearEvent(int campaignId, bool isFirstClear = false)
	{
		CampaignId = campaignId;
		IsFirstClear = isFirstClear;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal))
		{
			if (eventCondition.Type == EEventConditionType.CampaignClear)
			{
				if (eventCondition.Value1 < 1 || CampaignId == eventCondition.Value1)
				{
					return Math.Min(currentProgress + 1, goal);
				}
			}
			else if (eventCondition.Type == EEventConditionType.CampaignCleared && (eventCondition.Value1 < 0 || CampaignId == eventCondition.Value1))
			{
				return 1L;
			}
		}
		return currentProgress;
	}
}
