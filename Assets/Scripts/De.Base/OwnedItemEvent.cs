using GameCommon.Enum;
using System;
using GameCommon.Data;

public class OwnedItemEvent : IBaseEvent
{
	public int ItemId { get; private set; }

	public long ItemCountInInventory { get; private set; }

	public long ItemCountInStash { get; private set; }

	public long TeamItemCountInInventory { get; private set; }

	public OwnedItemEvent(int itemId, long itemCountInInventory, long itemCountInStash, long teamItemCountInInventory)
	{
		ItemId = itemId;
		ItemCountInInventory = itemCountInInventory;
		ItemCountInStash = itemCountInStash;
		TeamItemCountInInventory = teamItemCountInInventory;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		if (eventCondition.CanCalculateProgress(currentProgress, goal))
		{
			long num;
			if (eventCondition.Type == EEventConditionType.MyItemCountInInventory && ItemId == eventCondition.Value1)
			{
				num = ItemCountInInventory;
			}
			else if (eventCondition.Type == EEventConditionType.MyItemCountTotal && ItemId == eventCondition.Value1)
			{
				return Math.Min(ItemCountInInventory + ItemCountInStash, goal);
			}
			else
			{
				if (eventCondition.Type != EEventConditionType.TeamItemCountInInventory || ItemId != eventCondition.Value1)
				{
					return currentProgress;
				}
				num = TeamItemCountInInventory;
			}
			currentProgress = Math.Min(num, goal);
		}
		return currentProgress;
	}
}
