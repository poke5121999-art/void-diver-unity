using GameCommon.Enum;
using System;
using GameCommon.Data;

public class ItemUseEvent : IBaseEvent
{
	public int ItemId { get; private set; }

	public ItemUseEvent(int itemId)
	{
		ItemId = itemId;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal))
		{
			if (eventCondition.Type == EEventConditionType.ItemUse)
			{
				if (eventCondition.Value1 < 1 || ItemId == eventCondition.Value1)
				{
					return Math.Min(currentProgress + 1, goal);
				}
			}
			else if (eventCondition.Type == EEventConditionType.ItemUsed && ItemId == eventCondition.Value1)
			{
				return 1L;
			}
		}
		return currentProgress;
	}
}
