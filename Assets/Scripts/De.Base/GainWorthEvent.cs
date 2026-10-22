using System;
using GameCommon.Enum;
using GameCommon.Data;

public class GainWorthEvent : IBaseEvent
{
	public int GainWorth { get; private set; }

	public int TeamTotalWorth { get; private set; }

	public GainWorthEvent(int gainWorth, int teamTotalWorth)
	{
		GainWorth = gainWorth;
		TeamTotalWorth = teamTotalWorth;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal))
		{
			if (eventCondition.Type == EEventConditionType.GainedWorth)
			{
				currentProgress = Math.Min(GainWorth, goal);
			}
			else if (eventCondition.Type == EEventConditionType.TeamTotalWorth)
			{
				currentProgress = Math.Min(TeamTotalWorth, goal);
			}
		}
		return currentProgress;
	}
}
