using System;
using GameCommon.Enum;
using GameCommon.Data;

public class GainCoinEvent : IBaseEvent
{
	public ECoinGainType GainType { get; private set; }

	public long Amount { get; private set; }

	public GainCoinEvent(ECoinGainType gainType, long amount)
	{
		GainType = gainType;
		Amount = amount;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal) && eventCondition.Type == EEventConditionType.GainedCoin && (eventCondition.Value1 < 0 || (long)GainType == eventCondition.Value1))
		{
			currentProgress = Math.Min(Amount + currentProgress, goal);
		}
		return currentProgress;
	}
}
