using GameCommon.Enum;
using System;
using GameCommon.Data;

public class OpenRewardBoxEvent : IBaseEvent
{
	public ulong SourceClientId { get; private set; }

	public int RewardBoxId { get; private set; }

	public OpenRewardBoxEvent(ulong sourceClientId, int rewardBoxId)
	{
		SourceClientId = sourceClientId;
		RewardBoxId = rewardBoxId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.OpenRewardBox && (RewardBoxId == 0 || RewardBoxId == eventCondition.Value1))
		{
			currentProgress = Math.Min(currentProgress + 1, goal);
		}
		return currentProgress;
	}
}
