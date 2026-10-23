using GameCommon.Enum;
using System;
using GameCommon.Data;

public class MissionClearEvent : IBaseEvent
{
	public int MissionId { get; private set; }

	public MissionClearEvent(int missionId)
	{
		MissionId = missionId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal))
		{
			if (eventCondition.Type == EEventConditionType.MissionClear)
			{
				if (eventCondition.Value1 < 1 || MissionId == eventCondition.Value1)
				{
					return Math.Min(currentProgress + 1, goal);
				}
			}
			else if (eventCondition.Type == EEventConditionType.MissionCleared && (eventCondition.Value1 < 0 || MissionId == eventCondition.Value1))
			{
				return 1L;
			}
		}
		return currentProgress;
	}
}
