using GameCommon.Enum;
using System;
using GameCommon.Data;

public class MonsterKillEvent : IBaseEvent
{
	public ulong SourceClientId { get; private set; }

	public int MonsterId { get; private set; }

	public MonsterKillEvent(ulong sourceClientId, int monsterId)
	{
		SourceClientId = sourceClientId;
		MonsterId = monsterId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.MonsterKill && (eventCondition.Value1 < 1 || MonsterId == eventCondition.Value1))
		{
			currentProgress = Math.Min(currentProgress + 1, goal);
		}
		return currentProgress;
	}
}
