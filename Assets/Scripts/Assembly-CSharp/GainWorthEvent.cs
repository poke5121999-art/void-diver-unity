using System;
using GameCommon.Data;
using GameCommon.Enum;

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
				return Math.Min(GainWorth, goal);
			}
			if (eventCondition.Type == EEventConditionType.TeamTotalWorth)
			{
				return Math.Min(TeamTotalWorth, goal);
			}
		}
		return currentProgress;
	}
}
