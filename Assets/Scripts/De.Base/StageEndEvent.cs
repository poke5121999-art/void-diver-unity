using System;
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
		CampaignId = campaignId;
		Difficulty = difficulty;
		Players = players;
		IsEscaped = isEscaped;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.StageEscape && IsEscaped && (eventCondition.Value1 < 1 || CampaignId == eventCondition.Value1) && (eventCondition.Value2 < 0 || (int)Difficulty == (int)eventCondition.Value2))
		{
			return Math.Min(currentProgress + 1, goal);
		}
		return currentProgress;
	}
}
